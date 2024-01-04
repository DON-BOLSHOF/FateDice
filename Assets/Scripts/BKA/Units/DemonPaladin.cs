using Cysharp.Threading.Tasks;
using Zenject;

namespace BKA.Units
{
    public class DemonPaladin : Unit
    {
        public sealed override UnitDefinition Definition { get; protected set; }

        public override void Execute()
        {
            
        }

        public DemonPaladin(DefinitionPool definitionPool)
        {
           Definition = definitionPool.GetFromPool("DemonPaladinDefinition");
        }
    }
}