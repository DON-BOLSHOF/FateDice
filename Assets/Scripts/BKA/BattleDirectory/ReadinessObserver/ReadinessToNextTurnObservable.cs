using System;
using BKA.BattleDirectory.BattleHandlers;
using UniRx;
using UnityEngine;
using Zenject;

namespace BKA.BattleDirectory.ReadinessObserver
{
    public class ReadinessToNextTurnObservable : IReadinessObservable, IInitializable, IDisposable
    {
        public ReadOnlyReactiveProperty<bool> IsReady { get; private set; }

        [Inject] private DiceHandler _diceHandler;
        [Inject] private FightHandler _fightHandler;

        public void Initialize()
        {
            IsReady = _diceHandler.IsDiceHandlerCompleteWork.CombineLatest(_fightHandler.IsReady, 
                (x,y) => x && y).ToReadOnlyReactiveProperty();
        }

        public void Dispose()
        {
            IsReady?.Dispose();
        }
    }
}