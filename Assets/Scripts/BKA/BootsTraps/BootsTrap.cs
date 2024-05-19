using BKA.Buffs;
using BKA.System;
using BKA.UI;
using UniRx;
using UnityEngine;
using Zenject;

namespace BKA.BootsTraps
{
    public class BootsTrap : MonoBehaviour
    {
        //[SerializeField] private DisclaimerPanel _disclaimerPanel;
        
        [Inject] private DefinitionPool _definitionPool;

        [Inject] private ArtefactPool _artefactPool;

        [Inject] private BootsTrapStateObserver _stateObserver;

        [Inject] private LevelManager _levelManager;

        public async void Start()
        {
            //_disclaimerPanel.OnEnded.Subscribe(_ => LoadLevel()).AddTo(this);
            
            if (_stateObserver.BootsTrapState.Value != BootsTrapState.Loaded)
            {
                await _definitionPool.UploadBaseDefinitions();
                await _artefactPool.UploadBaseDefinitions();

                _stateObserver.Visit(this);
            }
            
            LoadLevel();
        }

        private void LoadLevel()
        {
            _levelManager.LoadLevel("MainMenu");
        }
    }
}