using UnityEngine;
using Zenject;

namespace BKA.TestUploadData.Installers
{
    [CreateAssetMenu(menuName = "Installers/UploadData/UploadHandlerInstaller", fileName = "UploadHandlerInstaller")]
    public class UploadHandlerInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<UploadHandler>().AsSingle();
        }
    }
}