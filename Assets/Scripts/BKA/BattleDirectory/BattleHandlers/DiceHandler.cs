using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BKA.BattleDirectory.BattleSystems;
using BKA.Dices;
using BKA.System.Exceptions;
using BKA.Units;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;

namespace BKA.BattleDirectory.BattleHandlers
{
    public class UnitDice
    {
        public Vector3 PositionInBoard;
        public Vector3 BaseUnitPosition;

        public DiceObject DiceObject;

        public CancellationTokenSource DiceCancellationTokenSource;
        public ReactiveProperty<bool> IsMoving;
    }

    public class DiceHandler : MonoBehaviour
    {
        private readonly List<UnitDice> _partyDices = new();
        private readonly List<UnitDice> _enemyDices = new();

        [SerializeField] private RerollHandler _rerollHandler;
        [SerializeField] private DiceMovementHandler _diceMovementHandler;

        [Inject] private UnitBattleBehaviourUploader _behaviourUploader;
        [Inject] private Boarder _boarder;

        private ReactiveProperty<bool> _isDiceTurnEnd = new();

        private CancellationTokenSource _handlerSource = new();

        public ReadOnlyReactiveProperty<bool> IsDiceHandlerCompleteWork;
        public ReadOnlyReactiveProperty<bool> IsDicesInputUnlocked;

        private void Awake()
        {
            IsDiceHandlerCompleteWork = _rerollHandler.IsDicesReady
                .CombineLatest(_diceMovementHandler.IsMovementComplete, _isDiceTurnEnd,
                    (isDicesReady, isMovementComplete, isTurnEnd) => isDicesReady && isMovementComplete && isTurnEnd)
                .ToReadOnlyReactiveProperty();

            IsDicesInputUnlocked = _rerollHandler.IsDicesReady.CombineLatest(_diceMovementHandler.IsMovementComplete, (
                isRerollEnd, isMovementEnd) => isRerollEnd && isMovementEnd).ToReadOnlyReactiveProperty();
        }

        private void Start()
        {
            _behaviourUploader.OnUploadedBehaviour.Subscribe(value => BindNewDice(value.Item1, value.Item2))
                .AddTo(this);
            IsDicesInputUnlocked.Subscribe(LockDicesInput).AddTo(this);
        }

        public async UniTask HandleNextTurn(TurnState currentTurn, CancellationToken token)
        {
            _isDiceTurnEnd.Value = false;

            List<UnitDice> activeDices = currentTurn switch
            {
                TurnState.PartyTurn => _partyDices,
                TurnState.EnemyTurn => _enemyDices,
                _ => throw new ArgumentOutOfRangeException(nameof(currentTurn), currentTurn, null)
            };

            ChangeDices(currentTurn);
            await UniTask.Yield(cancellationToken: token);
            GenerateRandomPositionsOnBoard(activeDices, currentTurn);

            foreach (var activeDice in activeDices)
            {
                activeDice.DiceObject.UnSelectDice();
            }

            await UniTask.Yield(token);

            await UniTask.WhenAll(activeDices.Select(dice => dice.IsMoving.Where(value => !value)
                .ToUniTask(useFirstValue: true, cancellationToken: token)));

            await _rerollHandler.ForceAsyncReroll(token);

            await CharactersTurn(currentTurn, token, activeDices);

            _isDiceTurnEnd.Value = true;
        }

        public void SynchronizeDices(Vector3[] partyDicesBasePositions, Vector3[] enemyDicesBasePositions)
        {
            if (partyDicesBasePositions.Count() != _partyDices.Count ||
                enemyDicesBasePositions.Count() != _enemyDices.Count)
                throw new SynchronizationException("Mismatched dice count and its new positions");

            for (var i = 0; i < partyDicesBasePositions.Length; i++)
            {
                _partyDices[i].BaseUnitPosition = partyDicesBasePositions[i];
            }

            for (var i = 0; i < enemyDicesBasePositions.Length; i++)
            {
                _enemyDices[i].BaseUnitPosition = enemyDicesBasePositions[i];
            }
        }

        public void SetReadyDices()
        {
            foreach (var activeDice in _partyDices)
            {
                activeDice.DiceObject.SelectDice();
            }
        }

        public async UniTask ReturnDicesToBoard(CancellationToken handlerSourceToken)
        {
            _handlerSource?.Cancel();
            _handlerSource = new();

            var source = CancellationTokenSource.CreateLinkedTokenSource(handlerSourceToken, _handlerSource.Token);

            _isDiceTurnEnd.Value = false;

            foreach (var activeDice in _partyDices)
            {
                activeDice.DiceObject.UnSelectDice();
            }

            await CharactersTurn(TurnState.PartyTurn, source.Token, _partyDices);

            _isDiceTurnEnd.Value = true;
        }

        private void BindNewDice(IUnitOfBattle unitOfBattle, UnitSide side)
        {
            var unitDice = new UnitDice { DiceObject = unitOfBattle.DiceObject, IsMoving = new() };
            switch (side)
            {
                case UnitSide.Party:
                    _partyDices.Add(unitDice);
                    unitOfBattle.DiceObject.IsSelected.Skip(1)
                        .Subscribe(value => OnSelectedChanged(unitDice, value).Forget()).AddTo(this);
                    break;
                case UnitSide.Enemy:
                    _enemyDices.Add(unitDice);
                    unitOfBattle.DiceObject.IsSelected.Skip(1)
                        .Subscribe(value => OnSelectedChanged(unitDice, value).Forget()).AddTo(this);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(side), side, null);
            }
        }

        private async UniTask OnSelectedChanged(UnitDice unitDice, bool value)
        {
            unitDice.DiceCancellationTokenSource?.Cancel();

            unitDice.DiceCancellationTokenSource = new();

            if (value)
            {
                unitDice.IsMoving.Value = true;

                await _diceMovementHandler.MoveDiceToBase(unitDice, token: unitDice.DiceCancellationTokenSource.Token);
                unitDice.DiceObject.SetReadyToAct();

                unitDice.DiceObject.gameObject.SetActive(false);
                unitDice.IsMoving.Value = false;
            }
            else
            {
                unitDice.IsMoving.Value = true;

                unitDice.DiceObject.gameObject.SetActive(true);
                unitDice.DiceObject.SetUnReadyToAct();
                await _diceMovementHandler.MoveDiceToPositionInBoard(unitDice,
                    token: unitDice.DiceCancellationTokenSource.Token);

                unitDice.IsMoving.Value = false;
            }
        }

        private async UniTask CharactersTurn(TurnState currentTurn, CancellationToken token, List<UnitDice> activeDices)
        {
            switch (currentTurn)
            {
                case TurnState.PartyTurn:
                    await UniTask.WaitUntil(() => activeDices.Count(activeDice =>
                        activeDice.DiceObject.IsSelected.Value) == activeDices.Count, cancellationToken: token);

                    await UniTask.WhenAll(activeDices.Select(dice => dice.IsMoving.Where(value => !value)
                        .ToUniTask(useFirstValue: true, cancellationToken: token)));
                    break;
                case TurnState.EnemyTurn:
                    foreach (var activeDice in activeDices)
                    {
                        activeDice.DiceObject.SelectDice();
                    }

                    await UniTask.WhenAll(activeDices.Select(dice => dice.IsMoving.Where(value => !value)
                        .ToUniTask(useFirstValue: true, cancellationToken: token)));
                    foreach (var activeDice in activeDices)
                    {
                        activeDice.DiceObject.SelectDice();
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(currentTurn), currentTurn, null);
            }
        }

        private void ChangeDices(TurnState turnState)
        {
            switch (turnState)
            {
                case TurnState.PartyTurn:
                    foreach (var unitDice in _enemyDices)
                    {
                        unitDice.DiceObject.gameObject.SetActive(false);
                    }

                    foreach (var diceObject in _partyDices)
                    {
                        diceObject.DiceObject.gameObject.SetActive(true);
                    }

                    _rerollHandler.UpdateDices(_partyDices.Select(unitDice => unitDice.DiceObject).ToList());
                    break;
                case TurnState.EnemyTurn:
                    foreach (var diceObject in _enemyDices)
                    {
                        diceObject.DiceObject.gameObject.SetActive(true);
                    }

                    foreach (var diceObject in _partyDices)
                    {
                        diceObject.DiceObject.gameObject.SetActive(false);
                    }

                    _rerollHandler.UpdateDices(_enemyDices.Select(unitDice => unitDice.DiceObject).ToList());
                    break;
            }
        }

        private void LockDicesInput(bool value)
        {
            foreach (var partyDice in _partyDices)
            {
                partyDice.DiceObject.LockInput(!value);
            }

            foreach (var enemyDice in _enemyDices)
            {
                enemyDice.DiceObject.LockInput(!value);
            }
        }

        private void GenerateRandomPositionsOnBoard(List<UnitDice> activeDices, TurnState turnState)
        {
            var positionsToMove = _boarder.GenerateProportionalPositionsToMove(activeDices.Count, turnState);
            for (var i = 0; i < positionsToMove.Count; i++)
            {
                activeDices[i].PositionInBoard = positionsToMove[i];
            }
        }
    }
}