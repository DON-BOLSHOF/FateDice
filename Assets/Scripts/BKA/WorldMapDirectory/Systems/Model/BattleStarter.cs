using System;
using System.Linq;
using BKA.Buffs;
using BKA.System;
using BKA.UI.WorldMap;
using BKA.WorldMapDirectory.Systems.Interfaces;
using Cysharp.Threading.Tasks;
using UniRx;
using Unit = BKA.Units.Unit;

namespace BKA.WorldMapDirectory.Systems
{
    public class BattleStarter : IBattleStarter, IDisposable
    {
        private LevelManager _levelManager;
        private GameSession _gameSession;
        private IBattlePanel _battlePanel;

        private CompositeDisposable _activatorDisposable = new();

        public BattleStarter(IBattlePanel battlePanel, LevelManager levelManager, GameSession gameSession)
        {
            _levelManager = levelManager;
            _gameSession = gameSession;
            _battlePanel = battlePanel;
        }

        public async UniTaskVoid StartBattle(Unit[] enemies, int xpValue)
        {
            _battlePanel.SetData(enemies.Select(unit => unit.Definition).ToArray());

            _battlePanel.ActivatePanel();
            await _battlePanel.OnActivatedBattle.ToUniTask(useFirstValue: true);
            //_battlePanel.DeactivatePanel();

            _levelManager.LoadLevel("BattleScene", (container) =>
            {
                container.Bind<Unit[]>().WithId("Party").FromInstance(_gameSession.Party.ToArray()).AsCached();
                container.Bind<Unit[]>().WithId("Enemies").FromInstance(enemies.ToArray()).AsCached();

                container.Bind<int>().WithId("XP").FromInstance(xpValue).AsSingle();

                container.Bind<Artefact[]>().FromInstance(_gameSession.Artefacts.ToArray()).AsSingle();
            });
        }

        public void Dispose()
        {
            _activatorDisposable?.Dispose();
        }
    }
}