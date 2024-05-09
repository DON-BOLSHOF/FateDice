using System;
using UniRx;

namespace BKA.UI.WorldMap.Dialog.Interfaces
{
    public interface IDialogPanel
    {
        IObservable<Unit> OnInputNextTurn { get; }
        IObservable<Unit> OnCharSpawned { get; }
        void ActivateNewPhrase(CharacterPhrase characterPhrase);
        void ActivatePanel();
        void DeactivatePanel();
    }
}