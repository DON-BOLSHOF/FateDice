using UniRx;

namespace BKA.BattleDirectory
{
    public class TurnSystem
    {
        private ReactiveProperty<TurnState> _turnState = new();
        public IReadOnlyReactiveProperty<TurnState> TurnState => _turnState;

        private void NextTurn()
        {
            _turnState.Value = _turnState.Value == BattleDirectory.TurnState.PartyTurn? BattleDirectory.TurnState.EnemyTurn: BattleDirectory.TurnState.PartyTurn;
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