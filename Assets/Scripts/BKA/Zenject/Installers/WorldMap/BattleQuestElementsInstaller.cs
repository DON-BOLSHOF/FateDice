using BKA.WorldMapDirectory.Quest;
using UnityEngine;

namespace Zenject.WorldMap
{
    public class BattleQuestElementsInstaller : MonoInstaller
    {
        [SerializeField] private BattleQuestElement[] _battleQuestElements;

        public override void InstallBindings()
        {
            foreach (var battleQuestElement in _battleQuestElements)
            {
                Container.Bind<BattleQuestElement>().FromInstance(battleQuestElement).AsCached();
            }
        }
    }
}