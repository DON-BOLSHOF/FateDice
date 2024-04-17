using BKA.System;
using UnityEngine;
using Zenject;

namespace BKA.Zenject.Installers.CommonSystems
{
    [CreateAssetMenu(menuName = "Installers/Common/GameSessionInstaller", fileName = "GameSessionInstaller")]
    public class GameSessionInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<GameSession>().AsSingle();
        }
    }
}