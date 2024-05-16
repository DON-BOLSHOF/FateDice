using System;
using System.Collections.Generic;
using System.Linq;
using BKA.BattleDirectory.BattleHandlers;
using BKA.Units;
using UniRx;
using Zenject;
using Unit = BKA.Units.Unit;

namespace BKA.BattleDirectory.BattleSystems
{
    public class BattleFinilizer : IInitializable, IDisposable
    {
        [Inject] private FightHandler _fightHandler;
        [Inject] private IEnumerable<IFinilizerLockedSystem> _lockedSystems;
        [Inject] private LoseHandler _loseHandler;
        [Inject] private WinHandler _winHandler;
        [Inject] private MainHeroHolder _mainHeroHolder;

        private CompositeDisposable _finilizerDisposable = new();
        
        public void Initialize()
        {
            _fightHandler.OnFightEnd.Subscribe(EndBattle).AddTo(_finilizerDisposable);
        }

        private void EndBattle((Unit[] heroPack, Unit[] enemyPack) tuple)
        {
            var endStatus = tuple.heroPack.Length > 0 && tuple.heroPack.Contains(_mainHeroHolder.MainHero)
                ? FightEndStatus.PartyWin
                : FightEndStatus.EnemyWin;
            
            foreach (var finilizerLockedSystem in _lockedSystems)
            {
                finilizerLockedSystem.Lock();
            }
            
            switch (endStatus)
            {
                case FightEndStatus.PartyWin:
                    _winHandler.ManageWin(tuple.heroPack);
                    break;
                case FightEndStatus.EnemyWin:
                    _loseHandler.ActivateLosePanel();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Dispose()
        {
            _finilizerDisposable.Dispose();
        }
    }
}