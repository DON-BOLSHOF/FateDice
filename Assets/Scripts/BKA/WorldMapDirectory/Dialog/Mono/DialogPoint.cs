using System;
using BKA.WorldMapDirectory.Dialog.Interfaces;
using BKA.WorldMapDirectory.Dialog.Model;
using UniRx;
using UnityEngine;

namespace BKA.UI.WorldMap.Dialog
{
    public abstract class DialogPoint : MonoBehaviour, IDialogPoint, IDisposable
    {
        [SerializeField] private CharacterPhraseProvider[] _characterPhrases;

        public IObservable<Unit> OnActivatedDialog => _onActivatedDialog;

        public CharacterPhraseProvider[] CharacterPhraseProviders => _characterPhrases;
        public DialogPointData DialogPointData => _dialogPointData;
        
        private readonly ReactiveCommand _onActivatedDialog = new();
        private DialogPointData _dialogPointData = new();

        protected virtual void Start()
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

        protected void ActivateDialog()
        {
            _dialogPointData.IsDialogTriggered = true;
            _onActivatedDialog?.Execute();
            gameObject.SetActive(false);
        }
    }
}