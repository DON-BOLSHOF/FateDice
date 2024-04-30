using BKA.Buffs;
using BKA.System;
using UnityEngine;
using Zenject;

namespace BKA.BootsTraps
{
    public class BootsTrap : MonoBehaviour
    {
        [Inject] private DefinitionPool _definitionPool;

        [Inject] private ArtefactPool _artefactPool;

        [Inject] private BootsTrapStateObserver _stateObserver;

        [Inject] private LevelManager _levelManager;

        public async void Start()
        {
            if (_stateObserver.BootsTrapState.Value != BootsTrapState.Loaded)
            {
                await _definitionPool.UploadBaseDefinitions();
                await _artefactPool.UploadBaseDefinitions();

                _stateObserver.Visit(this);
            }

            _levelManager.LoadLevel("MainMenu");
        }
    }
}