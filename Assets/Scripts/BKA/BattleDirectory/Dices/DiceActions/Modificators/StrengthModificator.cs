using BKA.Units;

namespace BKA.Dices.DiceActions
{
    public class StrengthModificator : ActionModificator
    {
        private Characteristics _characteristics;

        public StrengthModificator(Characteristics characteristics)
        {
            _characteristics = characteristics;
        }

        public override int GetModificatorValue()
        {
            return _characteristics.Strength / 2;
        }
    }
}