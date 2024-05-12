using BKA.UI.WorldMap.Quest.Mono;
using UnityEngine;

namespace Zenject.WorldMap
{
    public class QuestPanelInstaller : MonoInstaller
    {
        [SerializeField] private QuestPanel _questPanel;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<QuestPanel>().FromInstance(_questPanel).AsSingle();
        }
    }
}