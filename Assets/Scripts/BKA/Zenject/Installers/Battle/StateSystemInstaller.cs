using BKA.BattleDirectory.BattleSystems;
using UnityEngine;
using Zenject;

namespace BKA.Zenject.Installers
{
    public class StateSystemInstaller : MonoInstaller
    {
        [SerializeField] private StateSystem _stateSystem;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<StateSystem>().FromInstance(_stateSystem).AsSingle();
        }
    }
}