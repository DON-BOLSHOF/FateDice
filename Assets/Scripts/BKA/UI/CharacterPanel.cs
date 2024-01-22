using BKA.Utils;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Unit = BKA.Units.Unit;

namespace BKA.UI
{
    public class CharacterPanel : MonoBehaviour
    {
        [SerializeField] private Image _portrait;
        [SerializeField] private Image _attribute;
        [SerializeField] private HealthWidget _healthWidget;

        public ReactiveCommand OnCharacterPanelClicked = new();

        public void Fulfill(Unit unit)
        {
            _portrait.sprite = unit.Definition.UnitIcon;
        }

        public Vector3 GetAttributePositionInWorldSpace()
        {
            return UIToWorldConverter.Convert(_attribute.GetComponent<RectTransform>()) + new Vector3(0, 0.5f, 0);
        }
    }
}