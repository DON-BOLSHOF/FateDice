namespace BKA.BattleDirectory.BattleSystems
{
    public interface ITurnSystemVisitor
    {
        void Accept(TurnSystem turnSystem);
    }
}