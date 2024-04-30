using Zenject;

namespace BKA.BattleDirectory.BattleSystems
{
    public class XPAwarding : IXPAwarding
    {
        public int XPAward { get; }

        public XPAwarding([InjectOptional(Id = "XP")] int xpValue)
        {
            XPAward = xpValue;
        }
    }
}