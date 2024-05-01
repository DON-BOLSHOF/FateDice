using BKA.Units;

namespace BKA.Dices.DiceActions
{
    public class IntelligentModificator : ActionModificator
    {
        private Characteristics _characteristics;
        
        public IntelligentModificator(Characteristics characteristics)
        {
            _characteristics = characteristics;
        }
        
        public override int GetModificatorValue()
        {
            return _characteristics.Intelligent / 2;
        }
    }
}