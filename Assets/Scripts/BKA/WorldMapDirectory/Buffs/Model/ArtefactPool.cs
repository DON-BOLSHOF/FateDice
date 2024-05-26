using System;
using System.Collections.Generic;
using BKA.System;
using Cysharp.Threading.Tasks;

namespace BKA.Buffs
{
    public class ArtefactPool
    {
        private List<Artefact> _artefacts = new ();
        private List<ArtefactProvider> _unitDefinitionProviders = new();

        public async UniTask UploadBaseDefinitions()
        {
            var temp = new ArtefactProvider();
            _unitDefinitionProviders.Add(temp);
            
            var definition = await temp.Load("SwordFire");
            _artefacts.Add(definition); 
            definition = await temp.Load("SwordPoison");
            _artefacts.Add(definition); 
        }

        public void Dispose()
        {
            foreach (var unitDefinitionProvider in _unitDefinitionProviders)
            {
                unitDefinitionProvider.Unload();
            }
        }

        public Artefact GetFromPool(string definitionId)
        {
            if (TryGetArtefact(definitionId, out var artefact))
            {
                return artefact;
            }

            throw new ArgumentException("Definition is not in pull");
        }

        private bool TryGetArtefact(string definitionId, out Artefact artefact)
        {
            artefact = _artefacts.Find(def => def.ID.Equals(definitionId));

            return definitionId != null;
        }
    }
}