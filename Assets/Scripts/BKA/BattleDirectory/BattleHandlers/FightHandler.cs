using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BKA.BattleDirectory.BattleSystems;
using BKA.BattleDirectory.PlayerInput;
using BKA.BattleDirectory.ReadinessObserver;
using BKA.Dices.DiceActions;
using BKA.System;
using BKA.System.Exceptions;
using BKA.Units;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace BKA.BattleDirectory.BattleHandlers
{
    public class FightHandler : MonoBehaviour, IReadinessObservable
    {
        private readonly List<UnitBattleBehaviour> _firstPack = new();
        private readonly List<UnitBattleBehaviour> _secondPack = new();

        [Inject] private TurnSystem _turnSystem;

        [SerializeField] private DiceHandler _diceHandler;

        [SerializeField] private BattleInputHandler _battleInputHandler;

        [Inject] private UnitBattleBehaviourUploader _behaviourUploader;

        [Inject] private ReadinessToBattleObservable _readinessObservable;

        public ReadOnlyReactiveProperty<bool> IsReadyAbsolutely => _isReady.ToReadOnlyReactiveProperty();

        private readonly ReactiveProperty<bool> _isReady = new(true);

        private CancellationTokenSource _source;

        private void Start()
        {
            _behaviourUploader.OnUploadedBehaviour.Subscribe(value => UpdateUnits(value.Item1, value.Item2))
                .AddTo(this);
            
            _turnSystem.TurnState.Subscribe(_ => StartBattle().Forget()).AddTo(this);
        }

        private void UpdateUnits(UnitBattleBehaviour unit, UnitSide side)
        {
            switch (side)
            {
                case UnitSide.Party:
                    _firstPack.Add(unit);
                    break;
                case UnitSide.Enemy:
                    _secondPack.Add(unit);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(side), side, null);
            }
        }

        private async UniTask StartBattle()
        {
            _source?.Cancel();
            _source = new();
            
            _isReady.Value = false;
            
            await UniTask.WaitUntil(() => _readinessObservable.IsReadyAbsolutely.Value, cancellationToken: _source.Token);
         
            var currentTurn = _turnSystem.TurnState.Value;
            
            await _diceHandler.HandleNextTurn(currentTurn, _source.Token);

            switch (currentTurn)
            {
                case TurnState.PartyTurn:
                    await PlayerMove(_source.Token);
                    break;
                case TurnState.EnemyTurn:
                    await EnemyMove(_source.Token);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _isReady.Value = true;
        }

        private async UniTask PlayerMove(CancellationToken token)
        {
            await _battleInputHandler.MakeTurn(_firstPack, token).WithPostCancellation(ForceEndPlayerMove);
            Debug.Log("PlayerEndTurn");
        }

        private void ForceEndPlayerMove()
        {
            foreach (var unitBattleBehaviour in _firstPack)
            {
                unitBattleBehaviour.UnPrepareToAct();
            }
        }

        private async UniTask EnemyMove(CancellationToken token)
        {
            foreach (var unitBattleBehaviour in _secondPack)
            {
                int targetIndex;
                switch (unitBattleBehaviour.DiceAction.DiceActionData.DiceAttributeFocus)
                {
                    case DiceAttributeFocus.None:
                        break;
                    case DiceAttributeFocus.Enemy:
                        targetIndex = Random.Range(0, _firstPack.Count);
                        unitBattleBehaviour.DiceAction.ChooseTarget(_firstPack[targetIndex]);
                        break;
                    case DiceAttributeFocus.Ally:
                        targetIndex = Random.Range(0, _secondPack.Count);
                        unitBattleBehaviour.DiceAction.ChooseTarget(_secondPack[targetIndex]);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            await UniTask.Yield(cancellationToken: token);
            
            foreach (var unitBattleBehaviour in _secondPack)
            {
                await unitBattleBehaviour.Act(token);
            }
            Debug.Log("EnemyEndTurn");
        }

        public void OnDestroy()
        {
            _turnSystem?.Dispose();
            _readinessObservable?.Dispose();
            _isReady?.Dispose();
            _source?.Dispose();
        }
    }
}