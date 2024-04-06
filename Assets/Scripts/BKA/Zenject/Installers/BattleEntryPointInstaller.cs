using BKA.BattleDirectory;
using UnityEngine;
using Zenject;

namespace BKA.Zenject.Installers
{
    public class BattleEntryPointInstaller : MonoInstaller
    {
        [SerializeField] private BattleEntryPoint _battleEntryPoint;

        public override void InstallBindings()
        {
            Container.Bind<BattleEntryPoint>().FromInstance(_battleEntryPoint).AsSingle();
        }
    }
}