using UniRx;

namespace BKA.Units
{
    public class DemonPaladin : Unit
    {
        public sealed override UnitDefinition Definition { get; protected set; }
        protected sealed override ReactiveProperty<int> _health { get; } = new();

        public override void Execute()
        {
            
        }

        public DemonPaladin(DefinitionPool definitionPool)
        {
           Definition = definitionPool.GetFromPool("DemonPaladinDefinition");
           _health.Value = Definition.Health;
        }
    }
}