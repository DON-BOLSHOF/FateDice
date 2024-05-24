using System;
using System.Collections.Generic;
using BKA.System;
using BKA.Units;
using Cysharp.Threading.Tasks;

namespace BKA
{
    public class DefinitionPool : IDisposable
    {
        private List<UnitDefinition> _unitDefinitions = new ();
        private List<UnitDefinitionProvider> _unitDefinitionProviders = new();

        public async UniTask UploadBaseDefinitions()
        {
            var temp = new UnitDefinitionProvider();
            _unitDefinitionProviders.Add(temp);
            
            var definition = await temp.Load("DemonPaladinDefinition");
            _unitDefinitions.Add(definition); 
            definition = await temp.Load("HellMageDefinition");
            _unitDefinitions.Add(definition);
            definition = await temp.Load("FireMageDefinition");
            _unitDefinitions.Add(definition);
            definition = await temp.Load("RatDefinition");
            _unitDefinitions.Add(definition);
            definition = await temp.Load("WolfDefinition");
            _unitDefinitions.Add(definition);
        }

        public void Dispose()
        {
            foreach (var unitDefinitionProvider in _unitDefinitionProviders)
            {
                unitDefinitionProvider.Unload();
            }
        }

        public UnitDefinition GetFromPool(string definitionId)
        {
            if (TryGetDefinition(definitionId, out var def))
            {
                return def;
            }

            throw new ArgumentException("Definition is not in pull");
        }

        private bool TryGetDefinition(string definitionId, out UnitDefinition unitDefinition)
        {
            unitDefinition = _unitDefinitions.Find(def => def.ID.Equals(definitionId));

            return definitionId != null;
        }
    }
}