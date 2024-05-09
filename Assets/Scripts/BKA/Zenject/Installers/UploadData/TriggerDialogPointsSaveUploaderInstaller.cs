using BKA.UI.WorldMap.Dialog;
using UnityEngine;
using Zenject;

namespace BKA.TestUploadData.Installers
{
    [CreateAssetMenu(menuName = "Installers/UploadData/TriggerDialogPointsSaveUploaderInstaller", fileName = "TriggerDialogPointsSaveUploaderInstaller")]
    public class TriggerDialogPointsSaveUploaderInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<TriggerDialogPointsSaveUploader>().AsSingle().NonLazy();
        }
    }
}