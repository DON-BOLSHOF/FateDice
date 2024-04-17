using BKA.Units;
using UnityEngine;
using Zenject;

namespace BKA.Zenject.Installers.CommonSystems
{
    [CreateAssetMenu(menuName = "Installers/Common/UnitFactoryInstaller", fileName = "UnitFactoryInstaller")]
    public class UnitFactoryInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<UnitFactory>().AsSingle();
        }
    }
}