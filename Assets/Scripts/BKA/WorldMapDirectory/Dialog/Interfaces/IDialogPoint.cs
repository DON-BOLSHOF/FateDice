using System;
using BKA.UI.WorldMap.Dialog;
using UniRx;

namespace BKA.WorldMapDirectory.Dialog.Interfaces
{
    public interface IDialogPoint
    {
        IObservable<Unit> OnActivatedDialog { get; }
        CharacterPhraseProvider[] CharacterPhraseProviders { get; }
    }
}