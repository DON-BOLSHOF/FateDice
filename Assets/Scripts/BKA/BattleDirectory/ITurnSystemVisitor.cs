namespace BKA.BattleDirectory
{
    public interface ITurnSystemVisitor
    {
        void Accept(TurnSystem turnSystem);
    }
}