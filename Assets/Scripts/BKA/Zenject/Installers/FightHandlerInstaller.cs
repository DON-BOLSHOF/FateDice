using BKA.BattleDirectory.BattleHandlers;
using UnityEngine;
using Zenject;

namespace BKA.Zenject.Installers
{
    public class FightHandlerInstaller : MonoInstaller
    {
        [SerializeField] private FightHandler _fightHandler;
        
        public override void InstallBindings()
        {
            Container.Bind<FightHandler>().FromInstance(_fightHandler).AsSingle();
        }
    }
}