using BKA.UI.WorldMap.Dialog;
using UnityEngine;

namespace Zenject.WorldMap
{
    public class DialogPointsInstaller : MonoInstaller
    {
        [SerializeField] private TriggerDialogPoint[] _triggerDialogPoints;

        public override void InstallBindings()
        {
            foreach (var triggerDialogPoint in _triggerDialogPoints)
            {
                Container.BindInterfacesAndSelfTo<TriggerDialogPoint>().FromInstance(triggerDialogPoint).AsCached();
            }
        }
    }
}