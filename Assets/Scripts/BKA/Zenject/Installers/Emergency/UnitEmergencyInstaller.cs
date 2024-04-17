using BKA.Units;
using UnityEngine;
using Zenject;

namespace BKA.Zenject.Installers.Emergency
{
    [CreateAssetMenu(menuName = "Installers/Emergency/UnitEmergencyInstaller", fileName = "UnitEmergencyInstaller")]
    public class UnitEmergencyInstaller : ScriptableObjectInstaller
    {
        public override async void InstallBindings()
        {
            var resolve = Container.TryResolve<Unit>();

            if (resolve == null)
            {
                var definitionPool = Container.Resolve<DefinitionPool>();

                await definitionPool.UploadBaseDefinitions();
                Container.Bind<Unit>().FromInstance(new DemonPaladin(definitionPool)).AsCached();
            }
        }
    }
}