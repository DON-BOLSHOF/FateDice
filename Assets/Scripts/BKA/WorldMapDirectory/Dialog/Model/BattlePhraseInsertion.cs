using System;
using BKA.Units;
using BKA.WorldMapDirectory.Dialog.Interfaces;
using UnityEngine;

namespace BKA.UI.WorldMap.Dialog
{
    [Serializable]
    public class BattlePhraseInsertion : IPhraseInsertion
    {
        [SerializeField] private int _xp;
        [SerializeField] private UnitDefinition[] _unitDefinitions;
        
        public string CodeToInsert => "$Battle";
        public string Insertion => "Грядет битва";

        public int XP => _xp;
        public UnitDefinition[] UnitDefinitions => _unitDefinitions;
    }
}