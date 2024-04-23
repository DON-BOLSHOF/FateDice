using System;
using System.Threading;
using BKA.BattleDirectory;
using BKA.BattleDirectory.BattleHandlers;
using BKA.UI;
using Cysharp.Threading.Tasks;
using UniRx;
using Zenject;

namespace BKA.System
{
    public class BattleSynchronizer : IInitializable, IDisposable, ISynchronizer
    {
        [Inject] private CharacterBoarderHandler _boarderHandler;
        [Inject] private DiceHandler _diceHandler;

        [Inject] private BattleEntryPoint _battleEntryPoint;
        
        public ReadOnlyReactiveProperty<bool> IsSynchrolized { get; private set; }

        private CompositeDisposable _compositeDisposable = new();
        private CancellationTokenSource _UISyncCancellationToken = new();

        public void Initialize()
        {
            IsSynchrolized = _boarderHandler.IsSynchrolized.CombineLatest(_battleEntryPoint.IsReadyAbsolutely, 
                (x,y) => x && y).ToReadOnlyReactiveProperty();//Потом добавим еще

            _boarderHandler.IsSynchrolized.Where(value => !value).Subscribe(_ =>
            {
                _UISyncCancellationToken?.Cancel();
                _UISyncCancellationToken = new();
                
                SynchrolizeUI(_UISyncCancellationToken);
            }).AddTo(_compositeDisposable);
        }

        private async void SynchrolizeUI(CancellationTokenSource uiSyncCancellationToken)
        {
            await UniTask.DelayFrame(5, cancellationToken: uiSyncCancellationToken.Token);

            var positionsInUI = _boarderHandler.GetCurrentUIPositionsInWorldSpace();

            _diceHandler.SynchronizeDices(positionsInUI.partyBaseUIPositions, positionsInUI.enemyBaseUIPosition);

            _boarderHandler.Synchronize(this);
        }

        public void Dispose()
        {
            IsSynchrolized?.Dispose();
            _compositeDisposable.Clear();
        }
    }
}