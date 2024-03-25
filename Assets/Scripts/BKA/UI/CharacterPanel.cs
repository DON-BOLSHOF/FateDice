using BKA.Dices;
using BKA.Utils;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Unit = BKA.Units.Unit;

namespace BKA.UI
{
    public class CharacterPanel : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image _portrait;
        [SerializeField] private Image _attribute;
        [SerializeField] private HealthWidget _healthWidget;

        private DiceObject _diceObject;
        
        public void Fulfill(Unit unit, DiceObject dice)
        {
            _portrait.sprite = unit.Definition.UnitIcon;

            dice.OnDiceSelected.Subscribe(diceAction =>
            {
                _attribute.sprite = diceAction.DiceActionData.ActionView;
            }).AddTo(this);

            unit.Health.Subscribe(value => _healthWidget.SetHealth(value)).AddTo(this);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _diceObject.TryUnSelect();
        }

        public Vector3 GetAttributePositionInWorldSpace()
        {
            return UIToWorldConverter.Convert(_attribute.GetComponent<RectTransform>()) + new Vector3(0, 0.5f, 0);
        }
    }
}