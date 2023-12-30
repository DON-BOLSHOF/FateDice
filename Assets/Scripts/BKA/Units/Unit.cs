using BKA.System;

namespace BKA.Units
{
    public abstract class Unit
    {
        protected abstract UnitDefinition _definition { get; set; }

        protected UnitDefinitionProvider _definitionProvider { get; }

        public abstract void Execute();

        protected Unit()
        {
            _definitionProvider = new UnitDefinitionProvider();
        }

        ~Unit()
        {
            _definitionProvider.Unload();
        }
    }
}