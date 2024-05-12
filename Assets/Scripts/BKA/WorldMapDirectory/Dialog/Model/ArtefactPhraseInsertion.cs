using System;
using BKA.Buffs;
using BKA.WorldMapDirectory.Dialog.Interfaces;
using UnityEngine;

namespace BKA.UI.WorldMap.Dialog
{
    public enum ArtefactPhraseInsertionState
    {
        Give,
        Take
    }
    
    [Serializable]
    public class ArtefactPhraseInsertion : IPhraseInsertion
    {
        [SerializeField] private Artefact _artefact;
        [SerializeField] private ArtefactPhraseInsertionState _artefactPhraseInsertionState;
        
        public string CodeToInsert => "$ArtefactInsertion";
        public string Insertion => _artefact.ID;
        
        public Artefact Artefact => _artefact;
        public ArtefactPhraseInsertionState ArtefactPhraseInsertionState => _artefactPhraseInsertionState;
    }
}