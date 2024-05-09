using BKA.System.ExtraDirectory;
using BKA.Zenject.Signals;
using UnityEngine;
using Zenject;

namespace BKA.Audio
{
    public class SFXHandler : IAudioHandler
    {
        private AudioSource _audioSource;
        private SignalBus _signalBus;
        
        public SFXHandler(AudioSource audioSource, SignalBus signalBus)
        {
            _audioSource = audioSource;
            
            signalBus.Subscribe<SFXClipSignal>(ActivateClip);
        }

        private void ActivateClip(SFXClipSignal clipSignal)
        {
            _audioSource.PlayOneShot(clipSignal.AudioClip);
        }
    }
}