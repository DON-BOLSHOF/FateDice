using System.Collections.Generic;
using BKA.Dices.DiceActions;
using BKA.Units;
using UnityEngine;

namespace BKA.UI.WorldMap
{
    public class Sweep : MonoBehaviour
    {
        [SerializeField] private SweepAttribute[] _sweepAttributes;

        public void UpdateData(Unit unit)
        {
            var characteristics = unit.Class.Characteristics;

            for (var i = 0; i < _sweepAttributes.Length; i++)
            {
                var modificator = new CharacteristicActionProvider(characteristics,
                                          unit.DiceActions[i].DiceActionMainAttribute).GetModificator().GetModificatorValue()
                                  + unit.DiceActions[i].BaseActValue; //Полная хуйня, хоть и идея правильная но выведи в отдельную структуру это все

                _sweepAttributes[i].UpdateData(unit.DiceActions[i].ActionView, modificator);
            }
        }

        public void MakeHint(List<DiceActionPair> specializationDiceActionPairs)
        {
            DeactivateHint();
            foreach (var specializationDiceActionPair in specializationDiceActionPairs)
            {
                _sweepAttributes[specializationDiceActionPair.Index]
                    .MakeHint(specializationDiceActionPair.DiceAction.ActionView);
            }
        }

        public void DeactivateHint()
        {
            foreach (var sweepAttribute in _sweepAttributes)
            {
                sweepAttribute.DeactivateHint();
            }
        }
    }
}