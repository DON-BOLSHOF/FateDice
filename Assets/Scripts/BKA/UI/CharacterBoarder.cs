﻿using System;
using System.Collections.Generic;
using BKA.Units;
using UniRx;
using UnityEngine;
using Unit = BKA.Units.Unit;

namespace BKA.UI
{
    public class CharacterBoarder : MonoBehaviour
    {
        [SerializeField] private CharacterPanel[] _characterPanels;

        private const int MaximumPanels = 5;

        public ReactiveCommand<CharacterPanel> OnCharacterClicked = new();
        public ReactiveCommand<CharacterPanel> OnCharacterDead = new();

        private void Awake()
        {
            if (_characterPanels.Length > MaximumPanels) 
                throw new ArgumentException("too huge count of charactersPanels");
        }


        public void DynamicInit(List<UnitBattleBehaviour> characters)
        {
            if (characters.Count > _characterPanels.Length)
                throw new ArgumentException("too huge count of characters");

            var iterator = 0;
            
            for (; iterator < characters.Count; iterator++)
            {
                _characterPanels[iterator].gameObject.SetActive(true);
                _characterPanels[iterator].Fulfill(characters[iterator].Unit);
            }

            for (; iterator < _characterPanels.Length; iterator++)
            {
                _characterPanels[iterator].gameObject.SetActive(false);
            }
        }
    }
}