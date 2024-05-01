using System.Collections.Generic;
using BKA.Dices.DiceActions;
using BKA.Units;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BKA.Buffs
{
    [CreateAssetMenu(menuName = "Defs/Artefact", fileName = "Artefact")]
    public class Artefact : ScriptableObject, IBuff
    {
        [SerializeField] private string _id;
        [SerializeField] private BuffStatus _statusOfBuff;

        [SerializeField] private Sprite _view;

        [ShowIf("@(this._statusOfBuff & BuffStatus.Actions) == BuffStatus.Actions"), SerializeField]
        private List<DiceActionPair> _diceActions;

        [ShowIf("@(this._statusOfBuff & BuffStatus.Characteristics) == BuffStatus.Characteristics"), SerializeField]
        private Characteristics _characteristics;

        public string ID => _id;
        public Sprite View => _view;
        public BuffStatus StatusOfBuff => _statusOfBuff;
        public List<DiceActionPair> DiceActionPairs => _diceActions;
        public Characteristics Characteristics => _characteristics;
    }
}