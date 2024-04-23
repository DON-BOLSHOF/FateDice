using System;
using System.Collections.Generic;
using System.Linq;
using BKA.System;
using BKA.WorldMapDirectory.Artefacts;
using UniRx;
using Unit = BKA.Units.Unit;

namespace BKA.WorldMapDirectory.Systems
{
    public class BattleActivator : IDisposable
    {
        private LevelManager _levelManager;
        private GameSession _gameSession;

        private CompositeDisposable _activatorDisposable = new();

        public BattleActivator(IEnumerable<BattlePoint> battlePoints, ISynchronizer synchronizer,
            LevelManager levelManager, GameSession gameSession)
        {
            _levelManager = levelManager;
            _gameSession = gameSession;

            foreach (var battlePoint in battlePoints.Where(value => !value.BattlePointData.IsBattleBegan))
            {
                battlePoint.OnBattleStart.Where(_ => synchronizer.IsSynchrolized.Value).Subscribe(StartBattle)
                    .AddTo(_activatorDisposable);
            }
        }

        private void StartBattle(IEnumerable<Unit> enemies)
        {
            _levelManager.LoadLevel("BattleScene", (container) =>
            {
                container.Bind<Unit[]>().WithId("Party").FromInstance(_gameSession.Party.ToArray()).AsCached();
                container.Bind<Unit[]>().WithId("Enemies").FromInstance(enemies.ToArray()).AsCached();

                container.Bind<Artefact[]>().FromInstance(_gameSession.Artefacts.ToArray()).AsSingle();
            });
        }

        public void Dispose()
        {
            _activatorDisposable?.Dispose();
        }
    }
}