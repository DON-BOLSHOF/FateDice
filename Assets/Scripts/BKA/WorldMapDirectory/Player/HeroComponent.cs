﻿using BKA.WorldMapDirectory;
using BKA.Zenject.Signals;
using UnityEngine;
using Zenject;

namespace BKA.Player
{
    public class HeroComponent : MonoBehaviour
    {
        [SerializeField] private HeroPathFindingComponent _findingComponent;

        [Inject] private SignalBus _signalBus;
        [Inject] private PlayerInput _playerInput;
        
        private void Start()
        {
            _signalBus.Subscribe<BlockInputSignal>(OnBlockInput);
        }

        private void OnBlockInput(BlockInputSignal signal)
        {
            _playerInput.Block(signal.IsBlocked);
            
            if(signal.IsBlocked)
                _findingComponent.Stop();
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<BlockInputSignal>(OnBlockInput);
        }
    }
}