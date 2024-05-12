using BKA.WorldMapDirectory.Quest;
using UnityEngine;
using Zenject;

namespace BKA.TestUploadData.Installers
{
    [CreateAssetMenu(menuName = "Installers/UploadData/ActiveQuestSaveUploaderInstaller", fileName = "ActiveQuestSaveUploaderInstaller")]
    public class ActiveQuestSaveUploaderInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<ActiveQuestSaveUploader>().AsSingle().NonLazy();
        }
    }
}