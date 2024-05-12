using BKA.Player;
using UnityEngine;

namespace Zenject.WorldMap
{
    [CreateAssetMenu(menuName = "Installers/WorldMap/PlayerInputInstaller", fileName = "PlayerInputInstaller")]
    public class PlayerInputInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<PlayerInput>().AsSingle();
        }
    }
}