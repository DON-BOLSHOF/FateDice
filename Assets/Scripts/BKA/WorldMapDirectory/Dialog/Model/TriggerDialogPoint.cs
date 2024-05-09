using System;
using BKA.Player;
using BKA.WorldMapDirectory.Dialog.Interfaces;
using UniRx;
using UnityEngine;

namespace BKA.UI.WorldMap.Dialog
{
    [Serializable]
    public class DialogPointData
    {
        public bool IsDialogTriggered;
    }

    public class TriggerDialogPoint : MonoBehaviour, IDialogPoint, IDisposable
    {
        [SerializeField] private CharacterPhraseProvider[] _characterPhrases;

        public IObservable<Unit> OnActivatedDialog => _onActivatedDialog;

        public CharacterPhraseProvider[] CharacterPhraseProviders => _characterPhrases;
        public DialogPointData DialogPointData => _dialogPointData;
        
        private readonly ReactiveCommand _onActivatedDialog = new();
        private DialogPointData _dialogPointData = new();

        private void Start()
        {
            foreach (var characterPhraseProvider in _characterPhrases)
            {
                characterPhraseProvider.Initialize();
            }
        }

        public void DynamicInit(DialogPointData battlePointDataData)
        {
            _dialogPointData = battlePointDataData;

            if (_dialogPointData.IsDialogTriggered)
            {
                Dispose();
                gameObject.SetActive(false);
            }
        }

        public void Dispose()
        {
            _onActivatedDialog?.Dispose();
        }

        private void ActivateDialog()
        {
            _dialogPointData.IsDialogTriggered = true;
            _onActivatedDialog?.Execute();
            gameObject.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<HeroComponent>(out var heroComponent))
            {
                ActivateDialog();
            }
        }
    }
}