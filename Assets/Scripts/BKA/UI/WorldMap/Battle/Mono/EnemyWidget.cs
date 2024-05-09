using BKA.Units;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BKA.UI.WorldMap
{
    public class EnemyWidget : MonoBehaviour
    {
        [SerializeField] private Image _view;
        [SerializeField] private TextMeshProUGUI _description;

        public void SetData(UnitDefinition unitDefinition)
        {
            _view.sprite = unitDefinition.UnitIcon;
            _description.text = unitDefinition.ID;
        }
    }
}