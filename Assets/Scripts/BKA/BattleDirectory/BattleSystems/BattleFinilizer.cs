using System;
using System.Collections.Generic;
using BKA.BattleDirectory.BattleHandlers;
using BKA.Units;
using UniRx;
using UnityEngine;
using Zenject;

namespace BKA.BattleDirectory.BattleSystems
{
    public class BattleFinilizer : IInitializable, IDisposable
    {
        [Inject] private FightHandler _fightHandler;
        [Inject] private IEnumerable<IFinilizerLockedSystem> _lockedSystems;
        [Inject] private LoseHandler _loseHandler;

        private CompositeDisposable _finilizerDisposable = new();
        
        public void Initialize()
        {
            _fightHandler.OnFightEnd.Subscribe(EndBattle).AddTo(_finilizerDisposable);
        }

        private void EndBattle((FightEndStatus, List<UnitBattleBehaviour>) valueTuple)
        {
            var fightStatus = valueTuple.Item1;
            var partyPack = valueTuple.Item2;
            
            foreach (var finilizerLockedSystem in _lockedSystems)
            {
                finilizerLockedSystem.Lock();
            }
            
            switch (fightStatus)
            {
                case FightEndStatus.PartyWin:
                    Debug.Log("PartyWin");
                    break;
                case FightEndStatus.EnemyWin:
                    Debug.Log("EnemyWin");
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