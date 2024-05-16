using System;
using BKA.Units;
using BKA.WorldMapDirectory.Dialog.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace BKA.UI.WorldMap.Dialog
{
    public enum CharacterPhraseState
    {
        None,
        ArtefactHolder,
        HeroHolder,
        BattleHolder
    }

    [Serializable]
    public class CharacterPhraseProvider : IInitializable
    {
        [SerializeField] private CharacterPhrase _characterPhrase;
        [SerializeField] private CharacterPhraseState _characterPhraseState;

        [SerializeField, ShowIf("_characterPhraseState", CharacterPhraseState.ArtefactHolder)]
        private ArtefactPhraseInsertion _artefactInsertion;
        
        [SerializeField, ShowIf("_characterPhraseState", CharacterPhraseState.HeroHolder)]
        private HeroPhraseInsertion _heroInsertion;

        [SerializeField, ShowIf("_characterPhraseState", CharacterPhraseState.BattleHolder)]
        private BattlePhraseInsertion _battleInsertion;

        public CharacterPhraseState CharacterPhraseState => _characterPhraseState;
        public PhraseActor PhraseActor => _characterPhrase.Actor;
        public CharacterPhrase CharacterPhrase => _characterPhrase;

        public void Initialize()
        {
            switch (_characterPhraseState)
            {
                case CharacterPhraseState.None:
                    break;
                case CharacterPhraseState.ArtefactHolder:
                    _characterPhrase.DynamicInsertion(_artefactInsertion);
                    break;
                case CharacterPhraseState.HeroHolder:
                    _characterPhrase.DynamicInsertion(_heroInsertion);
                    break;
                case CharacterPhraseState.BattleHolder:
                    _characterPhrase.DynamicInsertion(_battleInsertion);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public void DynamicSetHeroPhrase(UnitDefinition unitDefinition)//Не должно быть здесь, создать отдельную сущность.
        {
            _characterPhrase.DynamicSetHeroPhrase(unitDefinition);
        }

        public IPhraseInsertion GetInsertion()
        {
            return _characterPhraseState switch
            {
                CharacterPhraseState.None => null,
                CharacterPhraseState.ArtefactHolder => _artefactInsertion,
                CharacterPhraseState.HeroHolder => _heroInsertion,
                CharacterPhraseState.BattleHolder => _battleInsertion,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}