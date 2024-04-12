using BKA.BattleDirectory.BattleSystems;
using UnityEngine;
using Zenject;

namespace BKA.Zenject.Installers
{
    public class LoseHandlerInstaller : MonoInstaller
    {
        [SerializeField] private LoseHandler _loseHandler;
        
        public override void InstallBindings()
        {
            Container.Bind<LoseHandler>().FromInstance(_loseHandler).AsSingle();
        }
    }
}