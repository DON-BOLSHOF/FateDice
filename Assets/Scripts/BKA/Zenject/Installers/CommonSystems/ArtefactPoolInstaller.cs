using UnityEngine;
using Zenject;

namespace BKA.Zenject.Installers.CommonSystems
{
    [CreateAssetMenu(menuName = "Installers/Common/ArtefactPoolInstaller", fileName = "ArtefactPoolInstaller")]
    public class ArtefactPoolInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ArtefactPool>().AsSingle();
        }
    }
}