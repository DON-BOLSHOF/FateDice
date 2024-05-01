using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BKA.UI.WorldMap
{
    public class AttributeWidget : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _value;
        [SerializeField] private Transform _backGround;
        [SerializeField] private Image _view;

        public void ModifyAttribute(Sprite view, int modificatorValue)
        {
            _view.sprite = view;

            if (modificatorValue > 0)
            {
                _value.text = modificatorValue.ToString();
                _value.gameObject.SetActive(true);
                _backGround.gameObject.SetActive(true);
            }
            else
            {
                _value.gameObject.SetActive(false);
                _backGround.gameObject.SetActive(false);
            }
        }

        public void ClearData()
        {
            _view.sprite = null;
            _value.gameObject.SetActive(false);
            _backGround.gameObject.SetActive(false);
        }
    }
}