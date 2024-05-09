using UnityEngine;
using Zenject;

namespace BKA.UI.WorldMap.Battle.Installers
{
    public class BattlePanelInstaller : MonoInstaller
    {
        [SerializeField] private BattlePanel _battlePanel;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<BattlePanel>().FromInstance(_battlePanel).AsSingle();
        }
    }
}