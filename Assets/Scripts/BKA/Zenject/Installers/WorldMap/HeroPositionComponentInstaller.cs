using BKA.WorldMapDirectory;
using UnityEngine;

namespace Zenject.WorldMap
{
    public class HeroPositionComponentInstaller : MonoInstaller
    {
        [SerializeField] private HeroPositionComponent _heroPositionComponent;
        
        public override void InstallBindings()
        {
            Container.Bind<HeroPositionComponent>().FromInstance(_heroPositionComponent).AsSingle();
        }
    }
}