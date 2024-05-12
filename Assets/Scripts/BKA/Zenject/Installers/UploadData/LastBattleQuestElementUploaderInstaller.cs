using UnityEngine;
using Zenject;

namespace BKA.TestUploadData.Installers
{
    [CreateAssetMenu(menuName = "Installers/UploadData/LastBattleQuestElementUploaderInstaller", fileName = "LastBattleQuestElementUploaderInstaller")]
    public class LastBattleQuestElementUploaderInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<LastBattleQuestElementUploader>().AsSingle().NonLazy();
        }
    }
}