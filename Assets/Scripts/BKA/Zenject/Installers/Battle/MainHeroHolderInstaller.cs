using BKA.BattleDirectory.BattleSystems;
using UnityEngine;
using Zenject;

namespace BKA.Zenject.Installers
{
    public class MainHeroHolderInstaller : MonoInstaller
    {
        [SerializeField] private MainHeroHolder _mainHeroHolder;

        public override void InstallBindings()
        {
            Container.Bind<MainHeroHolder>().FromInstance(_mainHeroHolder).AsSingle();
        }
    }
}