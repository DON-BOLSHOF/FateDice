using BKA.System.UploadData;
using UnityEngine;
using Zenject;

namespace BKA.TestUploadData.Installers
{
    [CreateAssetMenu(menuName = "Installers/Synchronizer/WorldMapSynchronizerInstaller", fileName = "WorldMapSynchronizerInstaller")]
    public class WorldMapSynchronizerInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<WorldMapSynchronizer>().AsSingle();
        }
    }
}