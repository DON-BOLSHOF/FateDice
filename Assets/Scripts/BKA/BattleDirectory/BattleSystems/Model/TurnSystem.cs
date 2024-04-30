using System;
using BKA.BattleDirectory.ReadinessObserver;
using UniRx;
using Zenject;

namespace BKA.BattleDirectory.BattleSystems
{
    public class TurnSystem : IInitializable, IDisposable
    {
        private ReactiveProperty<TurnState> _turnState = new();
        public IReadOnlyReactiveProperty<TurnState> TurnState => _turnState;

        [Inject] private ReadinessToNextTurnObservable _readinessToNext;

        private CompositeDisposable _systemDisposable = new();
        
        public void Initialize()
        {
            _readinessToNext.IsReadyAbsolutely.Skip(1).Where(value => value).Subscribe(_ => NextTurn()).AddTo(_systemDisposable);
        }

        public void Visit(ITurnSystemVisitor visitor)
        {
            NextTurn();
        }

        private void NextTurn()
        {
            if(!_readinessToNext.IsReadyEmergency.Value)
                return;
            
            _turnState.Value = _turnState.Value == BattleSystems.TurnState.PartyTurn? BattleSystems.TurnState.EnemyTurn: BattleSystems.TurnState.PartyTurn;
        }

        public void Dispose()
        {
            _systemDisposable?.Dispose();
            _turnState?.Dispose();
            _readinessToNext?.Dispose();
        }
    }

    public enum TurnState
    {
        PartyTurn,
        EnemyTurn
    }
}