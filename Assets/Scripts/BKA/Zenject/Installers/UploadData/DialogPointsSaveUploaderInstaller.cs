using BKA.UI.WorldMap.Dialog;
using UnityEngine;
using Zenject;

namespace BKA.TestUploadData.Installers
{
    [CreateAssetMenu(menuName = "Installers/UploadData/DialogPointsSaveUploaderInstaller", fileName = "DialogPointsSaveUploaderInstaller")]
    public class DialogPointsSaveUploaderInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<DialogPointsSaveUploader>().AsSingle().NonLazy();
        }
    }
}