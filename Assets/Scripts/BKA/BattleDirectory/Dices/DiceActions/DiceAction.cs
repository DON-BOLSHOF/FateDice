using System;
using BKA.Units;

namespace BKA.Dices.DiceActions
{
    public class DiceAction
    {
        private DiceActionData _data;

        private UnitBattleBehaviour _target;

        private ActionModificator _actionModificator;
        
        public DiceActionData DiceActionData => _data;
        public int ActionModificatorValue => _actionModificator.GetModificatorValue() + _data.BaseActValue;

        public DiceAction(DiceActionData data, ActionModificator actionModificator)
        {
            _data = data;

            _actionModificator = actionModificator;
        }

        public void ChooseTarget(UnitBattleBehaviour target)
        {
            _target = target;
        }

        public void Act()
        {
            _data.Act(_target, _actionModificator);
        }

        public void Undo()
        {
            _data.Undo(_target, _actionModificator);
        }
    }
}