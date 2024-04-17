using UnityEngine;
using Zenject;

namespace BKA.Zenject.Installers
{
    [CreateAssetMenu(menuName = "Installers/Common/DefinitionPoolInstaller", fileName = "DefinitionPoolInstaller")]
    public class DefinitionPoolInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<DefinitionPool>().AsSingle();
        }
    }
}