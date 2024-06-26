﻿using BKA.BattleDirectory;
using BKA.BattleDirectory.BattleSystems;
using Zenject;

namespace BKA.Zenject.Installers
{
    public class TurnSystemInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<TurnSystem>().AsSingle();
        }
    }
}