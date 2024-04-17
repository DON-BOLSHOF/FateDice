using System;
using BKA.Dices.DiceActions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BKA.WorldMapDirectory.Artefacts
{
    [Flags]
    public enum ArtefactStatus
    {
        None,
        Actions,
        Characteristics
    }

    [Serializable]
    public class DiceActionPair
    {
        public int Index;
        public DiceActionData DiceAction;
    }

    [CreateAssetMenu(menuName = "Artefacts/Artefact", fileName = "Artefact")]
    public class Artefact : ScriptableObject
    {
        [SerializeField] private string _id;
        [SerializeField] private ArtefactStatus _artefactStatus;

        [SerializeField] private Sprite _view;

        [SerializeField, ShowIf("_artefactStatus", ArtefactStatus.Actions)]
        private DiceActionPair[] _diceActions;

        public string ID => _id;
        public Sprite View => _view;
        public ArtefactStatus ArtefactStatus => _artefactStatus;
        public DiceActionPair[] DiceActionPairs => _diceActions;
    }
}