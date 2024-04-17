using System;
using System.Collections.Generic;
using System.Linq;
using BKA.System;
using UniRx;
using Unit = BKA.Units.Unit;

namespace BKA.WorldMapDirectory.Systems
{
    public class BattleActivator: IDisposable
    {
        private LevelManager _levelManager;
        private GameSession _gameSession;
        
        private CompositeDisposable _activatorDisposable = new();
        
        public BattleActivator(IEnumerable<BattlePoint> battlePoints, LevelManager levelManager, GameSession gameSession)
        {
            _levelManager = levelManager;
            _gameSession = gameSession;
            
            foreach (var battlePoint in battlePoints)
            {
                battlePoint.OnBattleStart.Subscribe(StartBattle).AddTo(_activatorDisposable);
            }
        }

        private void StartBattle(IEnumerable<Unit> enemies)
        {
            _levelManager.LoadLevel("BattleScene", (container) =>
            {
                container.Bind<Unit[]>().WithId("Party").FromInstance(_gameSession.Party.ToArray()).AsCached();
                container.Bind<Unit[]>().WithId("Enemies").FromInstance(enemies.ToArray()).AsCached();
            });
        }

        public void Dispose()
        {
            _activatorDisposable?.Dispose();
        }
    }
}