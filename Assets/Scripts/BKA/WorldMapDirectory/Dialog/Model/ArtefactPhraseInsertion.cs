using System;
using BKA.Buffs;
using BKA.WorldMapDirectory.Dialog.Interfaces;
using UnityEngine;

namespace BKA.UI.WorldMap.Dialog
{
    [Serializable]
    public class ArtefactPhraseInsertion : IPhraseInsertion
    {
        [SerializeField] private Artefact _artefact;
        
        public string CodeToInsert => "$ArtefactInsertion";
        public string Insertion => _artefact.ID;
        
        public Artefact Artefact => _artefact;
    }
}