using System;
using BKA.Units;
using BKA.WorldMapDirectory.Dialog.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace BKA.UI.WorldMap.Dialog
{
    public enum PhraseActor
    {
        AnotherPerson,
        Hero
    }
    
    public enum PhraseActorPosition
    {
        Left,
        Right
    }

    [Serializable]
    public class CharacterPhrase
    {
        [SerializeField, ShowIf("_phraseActor", PhraseActor.AnotherPerson)] 
        private string _actorName;
       
        [SerializeField] private string _phrase;
       
        [SerializeField, ShowIf("_phraseActor", PhraseActor.AnotherPerson)]
        private Sprite _actorView;

        [SerializeField] private PhraseActor _phraseActor;
        
        [SerializeField] private PhraseActorPosition _phraseActorPosition;

        public string ActorName => _actorName;
        public string Phrase => _phrase;
        public Sprite ActorView => _actorView;
        public PhraseActorPosition PhraseActorPosition => _phraseActorPosition;
        public PhraseActor Actor => _phraseActor;
        
        public void DynamicSetHeroPhrase(UnitDefinition unitDefinition)//Не должно быть здесь, создать отдельную сущность.
        {
            _actorName = unitDefinition.ID;
            _actorView = unitDefinition.UnitIcon;
        }

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