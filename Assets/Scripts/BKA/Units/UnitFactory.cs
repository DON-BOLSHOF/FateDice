using System;
using Zenject;

namespace BKA.Units
{
    public class UnitFactory
    {
        [Inject] private DefinitionPool _definitionPool;
        
        public Unit UploadUnit(UnitDefinition unitDefinition)
        {
            return unitDefinition.ID switch // Переделай ибо полная залупа
            {
                "DemonPaladin" => new DemonPaladin(_definitionPool),
                "HellMage" => new HellMage(_definitionPool),
                _ => throw new ArgumentException("UnitFactory не поддерживает этот тип")
            };
        }
    }
}