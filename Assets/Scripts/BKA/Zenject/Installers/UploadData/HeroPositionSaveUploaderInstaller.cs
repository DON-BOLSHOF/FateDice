using BKA.WorldMapDirectory.Systems;
using UnityEngine;
using Zenject;

namespace BKA.TestUploadData.Installers
{
    [CreateAssetMenu(menuName = "Installers/UploadData/HeroPositionSaveUploaderInstaller", fileName = "HeroPositionSaveUploaderInstaller")]
    public class HeroPositionSaveUploaderInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<HeroPositionSaveUploader>().AsSingle().NonLazy();
        }
    }
}