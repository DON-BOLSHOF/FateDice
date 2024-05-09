using System;
using System.Collections.Generic;
using System.Linq;
using BKA.System;
using BKA.Units;
using BKA.WorldMapDirectory.Systems.Interfaces;
using BKA.Zenject.Signals;
using UniRx;
using Zenject;

namespace BKA.WorldMapDirectory.Systems
{
    public class BattleHandler : IDisposable
    {
        private SignalBus _signalBus;
        private IBattleStarter _battleStarter;
        private UnitFactory _unitFactory;

        private CompositeDisposable _handlerDisposable = new();

        public BattleHandler(IBattleStarter battleStarter, IEnumerable<BattlePoint> battlePoints,
            UnitFactory unitFactory,
            ISynchronizer synchronizer, SignalBus signalBus)
        {
            _signalBus = signalBus;
            _unitFactory = unitFactory;
            _battleStarter = battleStarter;

            _signalBus.Subscribe<ExtraordinaryBattleSignal>(signal => StartBattle((signal.Enemies, signal.Xp)));

            foreach (var battlePoint in battlePoints.Where(value => !value.BattlePointData.IsBattleBegan))
            {
                battlePoint.OnBattleStart.Where(_ => synchronizer.IsSynchrolized.Value).Subscribe(StartBattle)
                    .AddTo(_handlerDisposable);
            }
        }

        private void StartBattle((IEnumerable<UnitDefinition> Enemies, int Xp) tuple)
        {
            _signalBus.Fire(new BlockInputSignal{IsBlocked = true});
            
            _battleStarter
                .StartBattle(
                    tuple.Enemies.Select(enemyDefenition => _unitFactory.UploadUnit(enemyDefenition)).ToArray(),
                    tuple.Xp)
                .Forget();
        }

        public void Dispose()
        {
            _handlerDisposable?.Dispose();
        }
    }
}