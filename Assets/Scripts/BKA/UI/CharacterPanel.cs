using System;
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
        [SerializeField] private Image _attribute;
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private HealthWidget _healthWidget;
        [SerializeField] private Transform _view;

        public ReactiveCommand<UnitBattleBehaviour> OnPanelClicked = new();
        public UnitBattleBehaviour UnitBattleBehaviour => _unitBattleBehaviour;

        private UnitBattleBehaviour _unitBattleBehaviour;
        private Vector3 _viewBasePosition;

        private void Start()
        {
            _viewBasePosition = _view.localPosition;
        }

        public void Fulfill(UnitBattleBehaviour unitBehaviour)
        {
            _portrait.sprite = unitBehaviour.Unit.Definition.UnitIcon;

            _name.text = unitBehaviour.Unit.Definition.ID;

            unitBehaviour.DiceObject.OnDiceSelected.Subscribe(diceAction =>
            {
                _attribute.sprite = diceAction.DiceActionData.ActionView;
            }).AddTo(this);

            unitBehaviour.Unit.Health.Subscribe(value => _healthWidget.SetHealth(value)).AddTo(this);

            unitBehaviour.IsReadyToAct.Subscribe(isReady =>
            {
                if (isReady)
                {
                    OnReadyToAct();
                }
                else
                {
                    OnUnReadyToAct();
                }
            }).AddTo(this);

            _unitBattleBehaviour = unitBehaviour;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Clicked");
            OnPanelClicked?.Execute(_unitBattleBehaviour);
            //_diceObject.TryUnSelect();
        }

        public Vector3 GetAttributePositionInWorldSpace()
        {
            return UIToWorldConverter.Convert(_attribute.GetComponent<RectTransform>()) + new Vector3(0, 0.5f, 0);
        }

        private void OnReadyToAct()
        {
            transform.localScale = new Vector3(1.25f, 1.25f, 1f);
        }

        private void OnUnReadyToAct()
        {
            transform.localScale = new Vector3(1, 1, 1f);
        }

        public void SetActing()
        {
            _view.localPosition += new Vector3(50f, 0, 0);
        }

        public void SetUnActing()
        {
            _view.localPosition = _viewBasePosition;
        }
    }
}