using System;
using System.Collections.Generic;
using System.Linq;
using BKA.System.Synchronization.Interfaces;
using BKA.Units;
using BKA.WorldMapDirectory.Systems.Interfaces;
using BKA.Zenject.Signals;
using UniRx;
using Zenject;

namespace BKA.WorldMapDirectory.Systems
{
    public class BattleHandler : IBattleHandler, IDisposable
    {
        private SignalBus _signalBus;
        private IBattleStarter _battleStarter;
        private UnitFactory _unitFactory;

        private CompositeDisposable _handlerDisposable = new();

        public BattleHandler(IBattleStarter battleStarter, IEnumerable<BattlePoint> battlePoints,
            UnitFactory unitFactory,
            IWorldMapSynchronizer synchronizer, SignalBus signalBus)
        {
            _signalBus = signalBus;
            _unitFactory = unitFactory;
            _battleStarter = battleStarter;

            _signalBus.Subscribe<ExtraordinaryBattleSignal>(signal => StartBattle(signal.Enemies, signal.Xp));

            foreach (var battlePoint in battlePoints.Where(value => !value.BattlePointData.IsBattleBegan))
            {
                battlePoint.OnBattleStart.Where(_ => synchronizer.IsSynchrolized.Value)
                    .Subscribe(turple => StartBattle(turple.Item1, turple.Item2))
                    .AddTo(_handlerDisposable);
            }
        }

        private void StartBattle(IEnumerable<UnitDefinition> enemies, int xp)
        {
            _signalBus.Fire(new BlockInputSignal { IsBlocked = true });

            _battleStarter
                .StartBattle(
                    enemies.Select(enemyDefenition => _unitFactory.UploadUnit(enemyDefenition)).ToArray(),
                    xp)
                .Forget();
        }

        public void Dispose()
        {
            _handlerDisposable?.Dispose();
        }
    }
}