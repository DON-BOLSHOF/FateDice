using BKA.UI.WorldMap.Dialog;
using UnityEngine;

namespace Zenject.WorldMap
{
    public class DialogPointsInstaller : MonoInstaller
    {
        [SerializeField] private DialogPoint[] _triggerDialogPoints;

        public override void InstallBindings()
        {
            foreach (var triggerDialogPoint in _triggerDialogPoints)
            {
                Container.BindInterfacesAndSelfTo<DialogPoint>().FromInstance(triggerDialogPoint).AsCached();
            }
        }
    }
}