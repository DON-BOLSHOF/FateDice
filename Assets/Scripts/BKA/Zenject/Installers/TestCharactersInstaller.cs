using BKA.Units;
using UnityEngine;
using Zenject;

namespace BKA.Zenject.Installers
{
    public class TestCharactersInstaller : MonoInstaller
    {
        [SerializeField] private UnitDefinition[] _teammates;
        [SerializeField] private UnitDefinition[] _enemies;
        
        public override void InstallBindings()
        {
            Container.Bind<UnitDefinition[]>().WithId("Teammates").FromInstance(_teammates).AsCached();
            Container.Bind<UnitDefinition[]>().WithId("Enemies").FromInstance(_enemies).AsCached();
        }
    }
}