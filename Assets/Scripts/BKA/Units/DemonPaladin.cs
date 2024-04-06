using UniRx;

namespace BKA.Units
{
    public class DemonPaladin : Unit
    {
        public sealed override UnitDefinition Definition { get; protected set; }
        protected sealed override ReactiveProperty<int> _health { get; } = new();

        public DemonPaladin(DefinitionPool definitionPool)
        {
           Definition = definitionPool.GetFromPool("DemonPaladin");
           _health.Value = Definition.Health;
        }
    }
}