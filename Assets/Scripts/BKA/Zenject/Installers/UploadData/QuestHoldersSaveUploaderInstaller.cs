using BKA.WorldMapDirectory.Quest;
using UnityEngine;
using Zenject;

namespace BKA.TestUploadData.Installers
{
    [CreateAssetMenu(menuName = "Installers/UploadData/QuestHoldersSaveUploader", fileName = "QuestHoldersSaveUploader")]
    public class QuestHoldersSaveUploaderInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<QuestHolderSaveUploader>().AsSingle().NonLazy();
        }
    }
}