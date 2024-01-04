using BKA.Units;
using UnityEngine;
using Zenject;

namespace BKA.Zenject.Installers
{
    public class TestCharactersInstaller : MonoInstaller
    {
        [Inject] private DefinitionPool _definitionPool;
        
        public override void InstallBindings()
        {
            Container.Bind<Unit[]>().WithId("Teammates").FromInstance(new Unit[]{new DemonPaladin(_definitionPool)}).AsCached();
            Container.Bind<Unit[]>().WithId("Enemies").FromInstance(new Unit[]{new DemonPaladin(_definitionPool)}).AsCached();
        }
    }
}