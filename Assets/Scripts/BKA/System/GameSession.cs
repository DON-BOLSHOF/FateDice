using System;
using System.Collections.Generic;
using System.Linq;
using BKA.Buffs;
using UniRx;
using UnityEngine;
using Zenject;
using Unit = BKA.Units.Unit;

namespace BKA.System
{
    public class GameSession : IDisposable
    {
        private List<Unit> _partyCompanions;
        private List<Artefact> _artefacts;

        public List<Unit> Party => _partyCompanions;
        public List<Artefact> Artefacts => _artefacts;

        public IObservable<UniRx.Unit> OnArtefactsUpdated => _onArtefactsUpdated;//Вынеси
        public IObservable<Unit> OnUnitUpdated => _onUnitUpdated;
        
        private readonly ReactiveCommand _onArtefactsUpdated = new();
        private readonly ReactiveCommand<Unit> _onUnitUpdated = new();

        private CompositeDisposable _sessionDisposable = new();
        
        public GameSession(IEnumerable<Unit> partyCompanions, [InjectOptional] IEnumerable<Artefact> artefacts)
        {
            _partyCompanions = partyCompanions.ToList();

            _artefacts = artefacts != null ? artefacts.ToList() : new();
            
            foreach (var partyCompanion in _partyCompanions)
            {
                partyCompanion.OnUpdatedData.Subscribe(_ => _onUnitUpdated?.Execute(partyCompanion)).AddTo(_sessionDisposable);
            }
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

        public void AddArtefact(Artefact artefact)
        {
            _artefacts.Add(artefact);

            _onArtefactsUpdated?.Execute();
        }

        public void RemoveArtefact(Artefact artefact)
        {
            _artefacts.Remove(artefact);

            _onArtefactsUpdated?.Execute();
        }

        public void Dispose()
        {
            _onArtefactsUpdated?.Dispose();
            _onUnitUpdated?.Dispose();
            _sessionDisposable?.Dispose();
        }
    }
}