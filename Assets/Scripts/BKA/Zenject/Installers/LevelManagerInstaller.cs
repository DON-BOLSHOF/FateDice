using BKA.System;
using UnityEngine;
using Zenject;

namespace BKA.Zenject.Installers
{
    public class LevelManagerInstaller : MonoInstaller
    {
        [SerializeField] private LevelManager _levelManager;
        
        public override void InstallBindings()
        {
            Container.Bind<LevelManager>().FromInstance(_levelManager).AsSingle();
        }
    }
}