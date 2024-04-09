using System;
using BKA.BattleDirectory.BattleHandlers;
using UniRx;
using Zenject;

namespace BKA.BattleDirectory.ReadinessObserver
{
    public class ReadinessToNextTurnObservable : IReadinessObservable, IInitializable, IDisposable
    {
        public ReadOnlyReactiveProperty<bool> IsReadyAbsolutely { get; private set; } // Для автоматизации
        public ReadOnlyReactiveProperty<bool> IsReadyEmergency { get; private set; } // Для внешних визиторов

        [Inject] private DiceHandler _diceHandler;
        [Inject] private FightHandler _fightHandler;

        public void Initialize()
        {
            IsReadyEmergency = _diceHandler.IsDiceHandlerCompleteWork.ToReadOnlyReactiveProperty();

            IsReadyAbsolutely = _diceHandler.IsDiceHandlerCompleteWork.CombineLatest(_fightHandler.IsReadyAbsolutely,
                (x, y) => x && y).ToReadOnlyReactiveProperty();
        }

        public void Dispose()
        {
            IsReadyAbsolutely?.Dispose();
        }
    }
}