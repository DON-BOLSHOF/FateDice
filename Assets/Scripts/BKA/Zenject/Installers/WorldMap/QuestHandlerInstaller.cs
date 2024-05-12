using BKA.WorldMapDirectory.Quest;
using UnityEngine;

namespace Zenject.WorldMap
{
    [CreateAssetMenu(menuName = "Installers/WorldMap/QuestHandlerInstaller", fileName = "QuestHandlerInstaller")]
    public class QuestHandlerInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<QuestHandler>().AsSingle().NonLazy();
        }
    }
}