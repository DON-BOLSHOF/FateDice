using BKA.System;
using Zenject;

namespace BKA.Units
{
    public abstract class Unit
    {
        public abstract UnitDefinition Definition { get; protected set; }

        [Inject] protected DefinitionPool _definitionPool;

        public abstract void Execute();
    }
}