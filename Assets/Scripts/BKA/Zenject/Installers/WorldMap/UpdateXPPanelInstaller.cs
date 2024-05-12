using BKA.UI;
using UnityEngine;

namespace Zenject.WorldMap
{
    public class UpdateXPPanelInstaller : MonoInstaller
    {
        [SerializeField] private UpdateXPPanel _panel;

        public override void InstallBindings()
        {
            Container.BindInterfacesTo<UpdateXPPanel>().FromInstance(_panel).AsSingle();
        }
    }
}