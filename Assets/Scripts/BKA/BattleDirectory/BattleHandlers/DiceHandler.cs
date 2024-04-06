using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
    public class DiceHandler : MonoBehaviour
    {
        private readonly List<UnitDice> _partyDices = new();
        private readonly List<UnitDice> _enemyDices = new();

        [SerializeField] private RerollHandler _rerollHandler;
        [SerializeField] private ReadyHandler _readyHandler;
        [SerializeField] private DiceMovementHandler _diceMovementHandler;

        [Inject] private UnitBattleBehaviourUploader _behaviourUploader;
        [Inject] private Boarder _boarder;

        public ReadOnlyReactiveProperty<bool> IsDiceHandlerCompleteWork;

        private void Awake()
        {
            IsDiceHandlerCompleteWork = _rerollHandler.IsDicesReady
                .CombineLatest(_diceMovementHandler.IsMovementComplete,
                    (isDicesReady, isMovementComplete) => isDicesReady && isMovementComplete)
                .ToReadOnlyReactiveProperty();

            _readyHandler.OnReady.Subscribe(_ => OnReadyClicked().Forget()).AddTo(this);
        }

        private async UniTask OnReadyClicked()
        {
            await _diceMovementHandler.MoveDicesToBase(_partyDices);
            foreach (var activeDice in _partyDices)
            {
                activeDice.DiceObject.SelectDice();
            }
        }

        private void Start()
        {
            _behaviourUploader.OnUploadedBehaviour.Subscribe(value => BindNewDice(value.Item1, value.Item2))
                .AddTo(this);
        }

        private void BindNewDice(IUnitOfBattle unitOfBattle, UnitSide side)
        {
            switch (side)
            {
                case UnitSide.Party:
                    _partyDices.Add(new UnitDice { DiceObject = unitOfBattle.DiceObject });
                    break;
                case UnitSide.Enemy:
                    _enemyDices.Add(new UnitDice { DiceObject = unitOfBattle.DiceObject });
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(side), side, null);
            }
        }

        public async UniTask HandleNextTurn(TurnState currentTurn, CancellationToken token)
        {
            List<UnitDice> activeDices = currentTurn switch
            {
                TurnState.PartyTurn => _partyDices,
                TurnState.EnemyTurn => _enemyDices,
                _ => throw new ArgumentOutOfRangeException(nameof(currentTurn), currentTurn, null)
            };

            ChangeDices(currentTurn);
            await UniTask.Yield(cancellationToken: token);
            GenerateRandomPositionsOnBoard(activeDices, currentTurn);
            await _diceMovementHandler.MoveDicesFromBase(activeDices, token);
            await _rerollHandler.ForceAsyncReroll(token);

            switch (currentTurn)
            {
                case TurnState.PartyTurn:
                    await UniTask.WhenAll(activeDices.Select(dice =>
                        dice.DiceObject.IsSelected.Where(value => value)
                            .ToUniTask(useFirstValue: true, cancellationToken: token)));
                    break;
                case TurnState.EnemyTurn:
                    await _diceMovementHandler.MoveDicesToBase(activeDices, token);
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
                        unitDice.DiceObject.TryUnSelect();
                        unitDice.DiceObject.gameObject.SetActive(false);
                    }

                    foreach (var diceObject in _partyDices)
                    {
                        diceObject.DiceObject.TryUnSelect();
                        diceObject.DiceObject.gameObject.SetActive(true);
                    }

                    _rerollHandler.UpdateDices(_partyDices.Select(unitDice => unitDice.DiceObject).ToList());
                    break;
                case TurnState.EnemyTurn:
                    foreach (var diceObject in _enemyDices)
                    {
                        diceObject.DiceObject.TryUnSelect();
                        diceObject.DiceObject.gameObject.SetActive(true);
                    }

                    foreach (var diceObject in _partyDices)
                    {
                        diceObject.DiceObject.TryUnSelect();
                        diceObject.DiceObject.gameObject.SetActive(false);
                    }

                    _rerollHandler.UpdateDices(_enemyDices.Select(unitDice => unitDice.DiceObject).ToList());
                    break;
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
    }

    public class UnitDice
    {
        public Vector3 PositionInBoard;
        public Vector3 BaseUnitPosition;
        public DiceObject DiceObject;
    }
}