using BKA.BattleDirectory.ReadinessObserver;
using UniRx;
using Zenject;

namespace BKA.BattleDirectory.BattleSystems
{
    public class TurnSystem
    {
        private ReactiveProperty<TurnState> _turnState = new();
        public IReadOnlyReactiveProperty<TurnState> TurnState => _turnState;

        [Inject] private ReadinessToNextTurnObservable _readinessToNext;

        private void NextTurn()
        {
            if(!_readinessToNext.IsReady.Value)
                return;
            
            _turnState.Value = _turnState.Value == BattleSystems.TurnState.PartyTurn? BattleSystems.TurnState.EnemyTurn: BattleSystems.TurnState.PartyTurn;
        }

        public void Visit(ITurnSystemVisitor visitor)
        {
            NextTurn();
        }
    }

    public enum TurnState
    {
        PartyTurn,
        EnemyTurn
    }
}