using System.Collections.Generic;
using BKA.Dices.DiceActions;
using BKA.Units;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BKA.Buffs
{
    public enum ArtefactState
    {
        Buff,
        Quest
    }
    
    [CreateAssetMenu(menuName = "Defs/Artefact", fileName = "Artefact")]
    public class Artefact : ScriptableObject, IBuff
    {
        [SerializeField] private string _id;
        [SerializeField] private Sprite _view;

        [SerializeField] private ArtefactState _artefactState;

        [ShowIf("_artefactState", ArtefactState.Buff), SerializeField]
        private BuffStatus _statusOfBuff;

        [ShowIf("@(this._statusOfBuff & BuffStatus.Actions) == BuffStatus.Actions"), SerializeField]
        private List<DiceActionPair> _diceActions;

        [ShowIf("@(this._statusOfBuff & BuffStatus.Characteristics) == BuffStatus.Characteristics"), SerializeField]
        private Characteristics _characteristics;

        public string ID => _id;
        public Sprite View => _view;
        public ArtefactState StateOfArtefact => _artefactState;
        public BuffStatus StatusOfBuff => _statusOfBuff;
        public List<DiceActionPair> DiceActionPairs => _diceActions;
        public Characteristics Characteristics => _characteristics;
    }
}