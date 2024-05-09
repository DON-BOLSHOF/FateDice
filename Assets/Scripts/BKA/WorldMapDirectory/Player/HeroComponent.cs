using System;
using BKA.System.ExtraDirectory;
using BKA.WorldMapDirectory;
using BKA.Zenject.Signals;
using UnityEngine;
using Zenject;

namespace BKA.Player
{
    public class HeroComponent : MonoBehaviour
    {
        [SerializeField] private HeroPathFindingComponent _findingComponent;

        [Inject] private SignalBus _signalBus;
        
        private void Start()
        {
            _signalBus.Subscribe<BlockInputSignal>(TryBlockInput);
        }

        private void TryBlockInput(BlockInputSignal signal)
        {
            if(signal.IsBlocked)
                _findingComponent.Stop();
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<BlockInputSignal>(TryBlockInput);
        }
    }
}