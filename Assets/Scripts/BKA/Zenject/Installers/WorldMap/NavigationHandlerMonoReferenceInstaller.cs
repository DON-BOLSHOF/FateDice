using BKA.System.Navigation.Mono;
using UnityEngine;
using Zenject;

namespace BKA.TestUploadData.Installers
{
    public class NavigationHandlerMonoReferenceInstaller : MonoInstaller
    {
        [SerializeField] private NavigationHandlerMonoReference _monoReference;
        
        public override void InstallBindings()
        {
            Container.Bind<NavigationHandlerMonoReference>().FromInstance(_monoReference).AsSingle();
        }
    }
}