using BKA.System.Navigation.Model;
using UnityEngine;
using Zenject;

namespace BKA.Zenject.Installers.WorldMap
{
    [CreateAssetMenu(menuName = "Installers/WorldMap/NavigationHandlerInstaller", fileName = "NavigationHandlerInstaller")]
    public class NavigationHandlerInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<NavigationHandler>().AsSingle().NonLazy();
        }
    }
}