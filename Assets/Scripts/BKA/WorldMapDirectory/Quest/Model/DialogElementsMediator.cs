using System;
using System.Collections.Generic;
using BKA.UI.WorldMap.Dialog;
using BKA.WorldMapDirectory.Dialog.Interfaces;
using Cysharp.Threading.Tasks;
using UniRx;

namespace BKA.WorldMapDirectory.Quest
{
    public class DialogElementsMediator : IDisposable
    {
        private IDialogHandler _dialogHandler;
        
        private CompositeDisposable _mediatorDisposable = new();
        
        public DialogElementsMediator(IEnumerable<TriggerDialogQuestElement> triggerDialogQuestElements,
            IDialogHandler dialogHandler)
        {
            _dialogHandler = dialogHandler;
            
            foreach (var triggerDialogQuestElement in triggerDialogQuestElements)
            {
                triggerDialogQuestElement.OnActivateDialog.Subscribe(ActivateDialog).AddTo(_mediatorDisposable);
            }
        }

        private async void ActivateDialog((CharacterPhraseProvider[] providers,Action onPostDialogAction) dialogTurple)
        {
            _dialogHandler.ForceActivateDialog(dialogTurple.providers);

            await _dialogHandler.OnDialogEnded.ToUniTask(useFirstValue: true);

            dialogTurple.onPostDialogAction();
        }

        public void Dispose()
        {
            _mediatorDisposable?.Dispose();
        }
    }
}