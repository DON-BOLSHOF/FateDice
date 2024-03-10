using System;
using BKA.BattleDirectory.BattleHandlers;
using UniRx;
using UnityEngine;
using Zenject;

namespace BKA.BattleDirectory.ReadinessObserver
{
    public class ReadinessToNextTurnObserver : IReadinessObserver, IInitializable, IDisposable
    {
        public ReadOnlyReactiveProperty<bool> IsReady { get; private set; }

        [Inject] private DiceHandler _diceHandler;
        
        public void Initialize()
        {
            Debug.Log("Init`");
            IsReady = _diceHandler.IsDiceHandlerCompleteWork.ToReadOnlyReactiveProperty();
        }

        public void Dispose()
        {
            IsReady?.Dispose();
        }
    }
}