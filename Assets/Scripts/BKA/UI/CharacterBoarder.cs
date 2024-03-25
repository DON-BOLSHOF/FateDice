using System;
using System.Linq;
using BKA.Units;
using UnityEngine;

namespace BKA.UI
{
    public class CharacterBoarder : MonoBehaviour
    {
        [SerializeField] private CharacterPanel[] _characterPanels;

        private const int MaximumPanels = 5;

        private void Awake()
        {
            if (_characterPanels.Length > MaximumPanels)
                throw new ArgumentException("too huge count of charactersPanels");

            foreach (var characterPanel in _characterPanels)
            {
                characterPanel.gameObject.SetActive(false);
            }
        }

        public void AddNewBehaviour(UnitBattleBehaviour unitBehaviour)
        {
            var needPanel = _characterPanels.FirstOrDefault(panel => !panel.gameObject.activeInHierarchy);
            if (needPanel == null)
            {
                throw new ArgumentException("There no position to add behaviour");
            }

            needPanel.gameObject.SetActive(true);
            needPanel.Fulfill(unitBehaviour.Unit, unitBehaviour.DiceObject);
        }

        public Vector3[] GetAttributePositionsInWorld()
        {
            var result = _characterPanels.Where(panel => panel.gameObject.activeInHierarchy)
                .Select(value => value.GetAttributePositionInWorldSpace()).ToArray();

            return result;
        }
    }
}