using UnityEngine;
using Zenject;

namespace BKA.Audio
{
    public class SFXHandlerInstaller : MonoInstaller
    {
        [SerializeField] private AudioSource _audioSource;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<SFXHandler>().AsSingle().WithArguments(_audioSource).NonLazy();
        }
    }
}