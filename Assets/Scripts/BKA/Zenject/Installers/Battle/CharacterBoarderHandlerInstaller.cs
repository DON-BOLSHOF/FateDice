using BKA.UI;
using UnityEngine;
using Zenject;

namespace BKA.Zenject.Installers
{
    public class CharacterBoarderHandlerInstaller : MonoInstaller
    {
        [SerializeField] private CharacterBoarderHandler _boarderHandler;
        
        public override void InstallBindings()
        {
            Container.Bind<CharacterBoarderHandler>().FromInstance(_boarderHandler).AsSingle();
        }
    }
}