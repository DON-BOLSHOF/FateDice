using BKA.Dices;
using UnityEngine;
using Zenject;

namespace BKA.Zenject.Installers
{
    public class DiceFactoryInstaller : MonoInstaller
    {
        [SerializeField] private DiceFactory _diceFactory;

        public override void InstallBindings()
        {
            Container.Bind<DiceFactory>().FromInstance(_diceFactory).AsSingle();
        }
    }
}