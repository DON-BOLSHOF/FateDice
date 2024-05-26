using BKA.System.UploadData;
using UnityEngine;
using Zenject;

namespace BKA.TestUploadData.Installers
{
    [CreateAssetMenu(menuName = "Installers/Synchronizer/NavigationSynchonizerInstaller", fileName = "NavigationSynchonizerInstaller")]
    public class NavigationSynchonizerInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<NavigationSynchronizer>().AsSingle();
        }
    }
}