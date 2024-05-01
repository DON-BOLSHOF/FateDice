using System;
using BKA.Units;

namespace BKA.Dices.DiceActions
{
    public class CharacteristicActionProvider : ActionModificatorProvider
    {
        private Characteristics _characteristics;
        private DiceActionMainAttribute _actionMainAttribute;

        public CharacteristicActionProvider(Characteristics characteristics,
            DiceActionMainAttribute actionMainAttribute)
        {
            _characteristics = characteristics;
            _actionMainAttribute = actionMainAttribute;
        }

        public override ActionModificator GetModificator()
        {
            return _actionMainAttribute switch
            {
                DiceActionMainAttribute.None => new NoneModificator(),
                DiceActionMainAttribute.Agility => new AgilityModificator(_characteristics),
                DiceActionMainAttribute.Strength => new StrengthModificator(_characteristics),
                DiceActionMainAttribute.Intelligent => new IntelligentModificator(_characteristics),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}