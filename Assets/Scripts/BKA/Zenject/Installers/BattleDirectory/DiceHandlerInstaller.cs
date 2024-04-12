using BKA.BattleDirectory;
using BKA.BattleDirectory.BattleHandlers;
using UnityEngine;
using Zenject;

namespace BKA.Zenject.Installers
{
    public class DiceHandlerInstaller : MonoInstaller
    {
        [SerializeField] private DiceHandler _diceHandler;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<DiceHandler>().FromInstance(_diceHandler).AsSingle();
        }
    }
}