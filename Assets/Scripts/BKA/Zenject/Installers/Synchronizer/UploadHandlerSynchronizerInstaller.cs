using BKA.System.UploadData;
using UnityEngine;
using Zenject;

namespace BKA.TestUploadData.Installers
{
    [CreateAssetMenu(menuName = "Installers/Synchronizer/UploadHandlerSynchronizerInstaller", fileName = "UploadHandlerSynchronizerInstaller")]
    public class UploadHandlerSynchronizerInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<UploadHandlerSynchronizer>().AsSingle();
        }   
    }
}