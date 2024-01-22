using BKA.BattleDirectory;
using UnityEngine;
using Zenject;

namespace BKA.Zenject.Installers
{
    public class BoarderInstaller : MonoInstaller
    {
        [SerializeField] private Boarder _boarder;
        
        public override void InstallBindings()
        {
            Container.Bind<Boarder>().FromInstance(_boarder).AsSingle();
        }
    }
}