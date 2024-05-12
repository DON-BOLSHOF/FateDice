using BKA.WorldMapDirectory.Quest;
using UnityEngine;

namespace Zenject.WorldMap
{
    public class DialogQuestElementsInstaller : MonoInstaller// Сомнительно, но будет жесткая связка с медиатором, что вроде допустимо 
    {
        [SerializeField] private TriggerDialogQuestElement[] _questElements;
        
        public override void InstallBindings()
        {
            foreach (var triggerDialogQuestElement in _questElements)
            {
                Container.Bind<TriggerDialogQuestElement>().FromInstance(triggerDialogQuestElement).AsCached().WhenInjectedInto<DialogElementsMediator>();
            }
        }
    }
}