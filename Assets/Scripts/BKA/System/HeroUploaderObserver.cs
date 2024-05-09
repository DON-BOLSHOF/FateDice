using System;
using BKA.Units;
using BKA.Zenject.Signals;
using Zenject;

namespace BKA.System
{
    public class HeroUploaderObserver : IDisposable
    {
        private UnitFactory _unitFactory;
        private SignalBus _signalBus;
        
        public HeroUploaderObserver(UnitFactory unitFactory, SignalBus signalBus)
        {
            _unitFactory = unitFactory;
            _signalBus = signalBus;
            
            _signalBus.Subscribe<UploadNewHeroSignal>(UploadNewHero);
        }

        private void UploadNewHero(UploadNewHeroSignal uploadNewHeroSignal)
        {
            var hero = _unitFactory.UploadUnit(uploadNewHeroSignal.HeroDefinition);
            
            _signalBus.Fire<UpdateNewHeroSignal>(new UpdateNewHeroSignal{Hero = hero});
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<UploadNewHeroSignal>(UploadNewHero);
        }
    }
}