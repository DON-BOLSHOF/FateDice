using System.Collections.Generic;
using BKA.Dices.DiceActions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BKA.Buffs
{
    [CreateAssetMenu(menuName = "Defs/Artefact", fileName = "Artefact")]
    public class Artefact : ScriptableObject, IBuff
    {
        [SerializeField] private string _id;
        [SerializeField] private BuffStatus _buffStatus;

        [SerializeField] private Sprite _view;

        [SerializeField, ShowIf("_buffStatus", BuffStatus.Actions)]
        private List<DiceActionPair> _diceActions;

        public string ID => _id;
        public Sprite View => _view;
        public BuffStatus BuffStatus => _buffStatus;
        public  List<DiceActionPair> DiceActionPairs => _diceActions;
    }
}