using System.Collections.Generic;
using BKA.Dices.DiceActions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BKA.Buffs
{
    [CreateAssetMenu(menuName = "Defs/Specialization/SpecializationDefinition", fileName = "SpecializationDefinition")]
    public class SpecializationDefinition: ScriptableObject
    {
        [SerializeField] private BuffStatus _buffStatus;

        [SerializeField] private Sprite _view;

        [SerializeField, ShowIf("_buffStatus", BuffStatus.Actions)]
        private List<DiceActionPair> _diceActions;

        public Sprite View => _view; 
        public BuffStatus BuffStatus => _buffStatus;
        public List<DiceActionPair> DiceAction => _diceActions;
    }
}