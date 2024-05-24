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
                "Феликс" => new DemonPaladin(_definitionPool),
                "Вельва" => new FireMage(_definitionPool),
                "Эдара" => new HellMage(_definitionPool),
                "Крыса" => new Rat(_definitionPool),
                "Волк" => new Wolf(_definitionPool),
                _ => throw new ArgumentException("UnitFactory не поддерживает этот тип")
            };
        }
    }
}