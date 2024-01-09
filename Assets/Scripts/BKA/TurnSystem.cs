using UniRx;

namespace BKA
{
    public class TurnSystem
    {
        private ReactiveProperty<TurnState> _turnState = new();
        public IReadOnlyReactiveProperty<TurnState> TurnState => _turnState;

        public void NextTurn()
        {
            _turnState.Value = _turnState.Value == BKA.TurnState.PartyTurn? BKA.TurnState.EnemyTurn: BKA.TurnState.PartyTurn;
        }
    }

    public enum TurnState
    {
        PartyTurn,
        EnemyTurn
    }
}