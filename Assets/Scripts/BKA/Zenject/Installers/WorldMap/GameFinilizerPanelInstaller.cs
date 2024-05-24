using BKA.UI;
using UnityEngine;

namespace Zenject.WorldMap
{
    public class GameFinilizerPanelInstaller : MonoInstaller
    {
        [SerializeField] private GameFinilizerPanel _gameFinilizerPanel;
        
        public override void InstallBindings()
        {
            Container.Bind<GameFinilizerPanel>().FromInstance(_gameFinilizerPanel).AsSingle();
        }
    }
}