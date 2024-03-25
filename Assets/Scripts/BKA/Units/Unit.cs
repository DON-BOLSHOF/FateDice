using BKA.System;
using Cysharp.Threading.Tasks;
using UniRx;

namespace BKA.Units
{
    public abstract class Unit
    {
        public abstract UnitDefinition Definition { get; protected set; }
        
        protected abstract ReactiveProperty<int> _health { get; }
        public IReadOnlyReactiveProperty<int> Health => _health;

        public void ModifyHealth(int value)
        {
            _health.Value += value;
        }
    }
}