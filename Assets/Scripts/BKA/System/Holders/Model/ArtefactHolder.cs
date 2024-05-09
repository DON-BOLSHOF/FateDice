using System;
using System.Collections.Generic;
using System.Linq;
using BKA.Buffs;
using UniRx;

namespace BKA.System.Holders.Model
{
    public class ArtefactHolder : IDisposable
    {
        public List<Artefact> Artefacts => _artefacts;
        public IObservable<Unit> OnArtefactsUpdated => _onArtefactsUpdated; //Вынеси
     
        private List<Artefact> _artefacts;
        private readonly ReactiveCommand _onArtefactsUpdated = new();

        public ArtefactHolder(IEnumerable<Artefact> units)
        {
            _artefacts = units.ToList();
        }

        public void UpdateArtefacts(List<Artefact> artefacts)
        {
            var localArtefacts = new List<Artefact>(_artefacts);

            foreach (var artefact in artefacts)
            {
                if (localArtefacts.Find(artef => artef.Equals(artefact)))
                {
                    localArtefacts.Remove(artefact);
                }
                else
                {
                    _artefacts.Add(artefact);
                }
            }

            foreach (var localArtefact in localArtefacts)
            {
                _artefacts.Remove(localArtefact);
            }

            _onArtefactsUpdated?.Execute();
        }
        
        public void Add(Artefact unit)
        {
            _artefacts.Add(unit);
            
            _onArtefactsUpdated?.Execute();
        }
        
        public void Remove(Artefact unit)
        {
            _artefacts.Remove(unit);
            
            _onArtefactsUpdated?.Execute();
        }

        public void Dispose()
        {
            _onArtefactsUpdated?.Dispose();
        }
    }
}