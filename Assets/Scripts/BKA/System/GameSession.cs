using System;
using System.Collections.Generic;
using BKA.Buffs;
using BKA.System.Holders.Model;
using BKA.Zenject.Signals;
using UniRx;
using Zenject;
using Unit = BKA.Units.Unit;

namespace BKA.System
{
    public class GameSession : IInitializable, IDisposable
    {
        public Unit MainHero => _mainHero;
        public List<Unit> Party => _unitsHolder.Units;
        public List<Artefact> Artefacts => _artefactHolder.Artefacts;

        public IObservable<UniRx.Unit> OnArtefactsHolderUpdated => _artefactHolder.OnArtefactsUpdated; 
        public IObservable<UniRx.Unit> OnUnitHolderUpdated => _unitsHolder.OnUnitHolderUpdated;
        public IObservable<Unit> OnUnitDataUpdated => _unitsHolder.OnUnitDataUpdated;

        private Unit _mainHero;
        private UnitsHolder _unitsHolder;
        private ArtefactHolder _artefactHolder;
        private SignalBus _signalBus;

        private CompositeDisposable _sessionDisposable = new();

        public GameSession([Inject(Id = "MainHero")] Unit mainHero, IEnumerable<Unit> partyCompanions, [InjectOptional] IEnumerable<Artefact> artefacts,
            SignalBus signalBus)
        {
            _mainHero = mainHero;
            _unitsHolder = new UnitsHolder(partyCompanions);
            _artefactHolder = new ArtefactHolder(artefacts ?? new List<Artefact>());
            _signalBus = signalBus;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<GiveArtefactSignal>(artefactSignal => _artefactHolder.Add(artefactSignal.Artefact));
            _signalBus.Subscribe<TakeArtefactSignal>(artefactSignal => _artefactHolder.Remove(artefactSignal.Artefact));
            _signalBus.Subscribe<UpdateNewHeroSignal>(heroSignal => _unitsHolder.Add(heroSignal.Hero));
        }

        public void UpdateArtefacts(List<Artefact> artefacts)//Сомнительно
        {
            _artefactHolder.UpdateArtefacts(artefacts);
        }

        public void Dispose()
        {
            _sessionDisposable?.Dispose();
        }
    }
}