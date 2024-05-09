using BKA.UI.WorldMap.Dialog;
using UnityEngine;

namespace Zenject.WorldMap
{
    public class DialogPanelInstaller : MonoInstaller
    {
        [SerializeField] private DialogPanel _dialogPanel;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<DialogPanel>().FromInstance(_dialogPanel).AsSingle();
        }
    }
}