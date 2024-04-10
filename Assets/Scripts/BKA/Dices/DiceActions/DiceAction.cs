using BKA.Units;

namespace BKA.Dices.DiceActions
{
    public class DiceAction
    {
        private DiceActionData _data;

        private UnitBattleBehaviour _target;
        
        public DiceActionData DiceActionData => _data;

        public DiceAction(DiceActionData data)
        {
            _data = data;
        }

        public void Act()
        {
            _data.Act(_target);
        }

        public void Undo()
        {
            _data.Undo(_target);
        }

        public void ChooseTarget(UnitBattleBehaviour target)
        {
            _target = target;
        }
    }
}