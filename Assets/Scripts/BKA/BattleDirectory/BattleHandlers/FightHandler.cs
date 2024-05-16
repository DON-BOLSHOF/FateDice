using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BKA.BattleDirectory.BattleSystems;
using BKA.BattleDirectory.PlayerInput;
using BKA.BattleDirectory.ReadinessObserver;
using BKA.Dices.DiceActions;
using BKA.System.Exceptions;
using BKA.Units;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;
using Unit = BKA.Units.Unit;

namespace BKA.BattleDirectory.BattleHandlers
{
    public enum FightEndStatus
    {
        PartyWin,
        EnemyWin
    }

    public class FightHandler : MonoBehaviour, IReadinessObservable, IFinilizerLockedSystem,IDisposable
    {
        private readonly List<UnitBattleBehaviour> _firstPack = new();
        private readonly List<UnitBattleBehaviour> _secondPack = new();

        [SerializeField] private DiceHandler _diceHandler;
        [SerializeField] private BattleInputHandler _battleInputHandler;

        [SerializeField] private ReadyHandler _readyHandler;
        [SerializeField] private UndoHandler _undoHandler;

        [Inject] private TurnSystem _turnSystem;
        [Inject] private UnitBattleBehaviourUploader _behaviourUploader;
        [Inject] private ReadinessToBattleObservable _readinessObservable;

        public ReadOnlyReactiveProperty<bool> IsReadyAbsolutely => _isReady.ToReadOnlyReactiveProperty();
        public IObservable<(Unit[], Unit[])> OnFightEnd => _onFightEnd;

        private readonly ReactiveProperty<bool> _isReady = new(true);
        private readonly ReactiveCommand<(Unit[], Unit[])> _onFightEnd = new();

        private CancellationTokenSource _handlerSource;
        private CompositeDisposable _handlerDisposable = new();
        
        private bool _isPlayerMove = false;

        public void Lock()
        {
            Dispose();   
        }

        private void Start()
        {
            _behaviourUploader.OnUploadedBehaviour.Subscribe(value => UpdateUnits(value.Item1, value.Item2))
                .AddTo(_handlerDisposable);

            _turnSystem.TurnState.Subscribe(_ => { StartBattle().Forget(); }).AddTo(_handlerDisposable);

            _readyHandler.OnReady.Subscribe(_ =>
            {
                if (!_diceHandler.IsDicesInputUnlocked.Value) return;

                _diceHandler.SetReadyDices();
            }).AddTo(_handlerDisposable);
            _undoHandler.OnUndo.Subscribe(_ =>
            {
                if (!_isPlayerMove) return;

                UndoPlayerMove();
            }).AddTo(_handlerDisposable);
        }

        private void UpdateUnits(UnitBattleBehaviour unit, UnitSide side)
        {
            switch (side)
            {
                case UnitSide.Party:
                    _firstPack.Add(unit);
                    unit.OnDead.Subscribe(_ =>
                    {
                        _firstPack.Remove(unit);
                        OnDeadUnitCheck();
                    }).AddTo(_handlerDisposable);
                    break;
                case UnitSide.Enemy:
                    _secondPack.Add(unit);
                    unit.OnDead.Subscribe(_ =>
                    {
                        _secondPack.Remove(unit);
                        OnDeadUnitCheck();
                    }).AddTo(_handlerDisposable);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(side), side, null);
            }
        }

        private async UniTask StartBattle()
        {
            _handlerSource?.Cancel();
            _handlerSource = new();

            _isReady.Value = false;

            await UniTask.WaitUntil(() => _readinessObservable.IsReadyAbsolutely.Value,
                cancellationToken: _handlerSource.Token);

            var currentTurn = _turnSystem.TurnState.Value;

            await _diceHandler.HandleNextTurn(currentTurn, _handlerSource.Token);

            await HandleCharacterTurn(currentTurn);
        }

        private async UniTask HandleCharacterTurn(TurnState currentTurn)
        {
            switch (currentTurn)
            {
                case TurnState.PartyTurn:
                    _isPlayerMove = true;
                    await PlayerMove(_handlerSource.Token);
                    _isPlayerMove = false;
                    break;
                case TurnState.EnemyTurn:
                    await EnemyMove(_handlerSource.Token);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _isReady.Value = true;
        }

        private async UniTask PlayerMove(CancellationToken token)
        {
            await _battleInputHandler.MakeTurn(_firstPack, token).WithPostCancellation(ForceEndPlayerMove);
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
                unitBattleBehaviour.Act();
            }
        }

        private void UndoPlayerMove()
        {
            if (_battleInputHandler._hasToUndo)
            {
                _battleInputHandler.UndoLastAct();
            }
            else
            {
                ReloadPlayerTurn().Forget();
            }
        }

        private async UniTask ReloadPlayerTurn()
        {
            _handlerSource?.Cancel();
            _handlerSource = new();

            await _diceHandler.ReturnDicesToBoard(_handlerSource.Token);
            await HandleCharacterTurn(TurnState.PartyTurn);
        }

        private void ForceEndPlayerMove()
        {
            foreach (var unitBattleBehaviour in _firstPack)
            {
                unitBattleBehaviour.UnPrepareToAct();
            }

            _isPlayerMove = false;
        }

        private void OnDeadUnitCheck()
        {
            if (_firstPack.Count > 0 && _secondPack.Count > 0) return;

            _handlerSource.Cancel();

            _onFightEnd?.Execute((_firstPack.Select(battleBehaviour => battleBehaviour.Unit).ToArray(),
                _secondPack.Select(battleBehaviour => battleBehaviour.Unit).ToArray()));
        }

        public void Dispose()
        {
            _turnSystem?.Dispose();
            _readinessObservable?.Dispose();
            _isReady?.Dispose();
            _handlerSource?.Dispose();
            _handlerDisposable?.Dispose();
        }
    }
}