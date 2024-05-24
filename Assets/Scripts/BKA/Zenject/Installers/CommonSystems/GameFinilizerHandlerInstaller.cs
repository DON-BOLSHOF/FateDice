using BKA.System;
using UnityEngine;
using Zenject;

namespace BKA.Zenject.Installers.CommonSystems
{
    [CreateAssetMenu(menuName = "Installers/Common/GameFinilizerHandlerInstaller", fileName = "GameFinilizerHandlerInstaller")]
    public class GameFinilizerHandlerInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameFinilizerHandler>().AsSingle().NonLazy();
        }
    }
}