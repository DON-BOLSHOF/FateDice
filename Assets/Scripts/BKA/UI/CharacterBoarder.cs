using System;
using UniRx;
using UnityEngine;

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
                throw new ArgumentException("too huge count of characters");
        }
        
        
    }
}