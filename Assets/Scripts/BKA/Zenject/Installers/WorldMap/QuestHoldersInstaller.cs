using BKA.WorldMapDirectory.Quest;
using UnityEngine;

namespace Zenject.WorldMap
{
    public class QuestHoldersInstaller : MonoInstaller
    {
        [SerializeField] private QuestHolder[] _questHolders;

        public override void InstallBindings()
        {
            foreach (var questHolder in _questHolders)
            {
                Container.Bind<QuestHolder>().FromInstance(questHolder).AsCached();
            }
        }
    }
}