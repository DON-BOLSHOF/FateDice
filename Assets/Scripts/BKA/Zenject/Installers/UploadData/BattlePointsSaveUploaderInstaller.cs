using BKA.WorldMapDirectory.Systems;
using UnityEngine;
using Zenject;

namespace BKA.TestUploadData.Installers
{
    [CreateAssetMenu(menuName = "Installers/UploadData/BattlePointsSaveUploaderInstaller", fileName = "BattlePointsSaveUploaderInstaller")]
    public class BattlePointsSaveUploaderInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<BattlePointsSaveUploader>().AsSingle().NonLazy();
        }
    }
}