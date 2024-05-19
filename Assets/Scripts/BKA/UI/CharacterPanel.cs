using System;
using BKA.UI.WorldMap;
using BKA.Units;
using BKA.Utils;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BKA.UI
{
    public class CharacterPanel : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image _portrait;
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private HealthWidget _healthWidget;
        [SerializeField] private AttributeWidget _attributeWidget;

        [SerializeField] protected Animator _characterPanelAnimator;

        public ReactiveCommand<UnitBattleBehaviour> OnPanelClicked = new();
        public UnitBattleBehaviour UnitBattleBehaviour => _unitBattleBehaviour;

        protected UnitBattleBehaviour _unitBattleBehaviour;
        private static readonly int Damaged = Animator.StringToHash("Damaged");
        private static readonly int Healed = Animator.StringToHash("Healed");

        public void Fulfill(UnitBattleBehaviour unitBehaviour)
        {
            _portrait.sprite = unitBehaviour.Unit.Definition.UnitIcon;

            _name.text = unitBehaviour.Unit.Definition.ID;

            unitBehaviour.DiceObject.OnDiceReadyToAct.Subscribe(diceAction =>
                _attributeWidget.ModifyAttribute(diceAction.DiceActionData.ActionView,
                    diceAction.ActionModificatorValue)
            ).AddTo(this);

            unitBehaviour.DiceObject.OnDiceUnReadyToAct.Subscribe(_ => { _attributeWidget.ClearData(); }).AddTo(this);

            unitBehaviour.Unit.Health.Subscribe(value => _healthWidget.SetHealth(value)).AddTo(this);

            unitBehaviour.OnDamaged.Subscribe(_ => _characterPanelAnimator.SetTrigger(Damaged)).AddTo(this);
            unitBehaviour.OnHealed.Subscribe(_ => _characterPanelAnimator.SetTrigger(Healed)).AddTo(this);

            unitBehaviour.IsReadyToAct.Subscribe(isReady =>
            {
                transform.localScale = isReady ? new Vector3(1.25f, 1.25f, 1f) : new Vector3(1, 1, 1f);
            }).AddTo(this);

            _unitBattleBehaviour = unitBehaviour;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnPanelClicked?.Execute(_unitBattleBehaviour);
        }

        public Vector3 GetAttributePositionInWorldSpace()
        {
            return UIToWorldConverter.Convert(_attributeWidget.GetComponent<RectTransform>()) + new Vector3(0, 0.5f, 0);
        }
    }
}