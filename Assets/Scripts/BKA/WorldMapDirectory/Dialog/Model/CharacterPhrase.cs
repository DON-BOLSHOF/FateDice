using System;
using BKA.WorldMapDirectory.Dialog.Interfaces;
using UnityEngine;

namespace BKA.UI.WorldMap.Dialog
{
    public enum FraseActorPosition
    {
        Left,
        Right
    }

    [Serializable]
    public class CharacterPhrase
    {
        [SerializeField] private string _actorName;
        [SerializeField] private string _phrase;
        [SerializeField] private Sprite _actor;
        [SerializeField] private FraseActorPosition phraseActorPosition;

        public string ActorName => _actorName;
        public string Phrase => _phrase;
        public Sprite Actor => _actor;
        public FraseActorPosition PhraseActorPosition => phraseActorPosition;

        public void DynamicInsertion(IPhraseInsertion phraseInsertion)
        {
            if (_phrase.Contains(phraseInsertion.CodeToInsert))
            {
                _phrase = _phrase.Replace(phraseInsertion.CodeToInsert, phraseInsertion.Insertion);
            }
            else
            {
                throw new ApplicationException("Нет кода вставки во фразе");
            }
        }
    }
}