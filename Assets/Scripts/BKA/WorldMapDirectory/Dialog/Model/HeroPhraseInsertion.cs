using System;
using BKA.Units;
using BKA.WorldMapDirectory.Dialog.Interfaces;
using UnityEngine;

namespace BKA.UI.WorldMap.Dialog
{
    [Serializable]
    public class HeroPhraseInsertion : IPhraseInsertion
    {
        [SerializeField] private UnitDefinition _heroDefition;

        public string CodeToInsert => "$Hero";
        public string Insertion => _heroDefition.ID;

        public UnitDefinition HeroDefinition => _heroDefition;
    }
}