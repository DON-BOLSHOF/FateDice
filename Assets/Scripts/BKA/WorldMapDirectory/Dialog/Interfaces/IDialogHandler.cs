using System;
using BKA.UI.WorldMap.Dialog;
using UniRx;

namespace BKA.WorldMapDirectory.Dialog.Interfaces
{
    public interface IDialogHandler
    {
        IObservable<Unit> OnDialogEnded { get; }
        void ForceActivateDialog(CharacterPhraseProvider[] phraseProviders);
    }
}