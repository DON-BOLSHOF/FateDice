using BKA.Units;

namespace BKA.Dices.DiceActions
{
    public class AgilityModificator: ActionModificator
    {
        private Characteristics _characteristics;
        
        public AgilityModificator(Characteristics characteristics)
        {
            _characteristics = characteristics;
        }
        
        public override int GetModificatorValue()
        {
            return _characteristics.Agility / 2;
        }
    }
}