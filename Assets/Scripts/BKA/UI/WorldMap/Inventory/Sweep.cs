using BKA.Dices.DiceActions;
using UnityEngine;
using UnityEngine.UI;

namespace BKA.UI.WorldMap
{
    public class Sweep : MonoBehaviour
    {
        [SerializeField] private Image[] _actions;

        public void UpdateData(DiceActionData[] actions)
        {
            for (var i = 0; i < _actions.Length; i++)
            {
                _actions[i].sprite = actions[i].ActionView;
            }
        }
    }
}