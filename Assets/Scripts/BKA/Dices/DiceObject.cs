using System;
using BKA.Dices.DiceActions;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BKA.Dices
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public abstract class DiceObject : MonoBehaviour, IPointerClickHandler
    {
        public abstract Rigidbody Rigidbody { get; protected set; }
        public abstract DiceAction[] DiceActions { get; protected set; }

        public ReactiveCommand<DiceAction> OnDiceReadyToAct = new();
        public ReactiveCommand OnDiceUnReadyToAct = new();

        public ReadOnlyReactiveProperty<bool> IsSelected => _isSelected.ToReadOnlyReactiveProperty();

        protected abstract int FixedActionsAmount { get; }
        protected abstract DiceEdge[] _diceEdges { get; set; }

        private Collider _collider;

        private ReactiveProperty<bool> _isSelected = new(true);

        private bool _lockedToInput = false;

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();

            if (FixedActionsAmount != _diceEdges.Length)
            {
                throw new ArgumentException("Edges amount is not equal fixed count!");
            }
        }

        public void LockInput(bool value)
        {
            _lockedToInput = value;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_lockedToInput)
            {
                SelectDice();
            }
        }

        public void SelectDice()
        {
            _isSelected.Value = true;
        }

        public void UnSelectDice()
        {
            _isSelected.Value = false;
        }

        public void SetReadyToAct()
        {
            for (var i = 0; i < _diceEdges.Length; i++)
            {
                if (_diceEdges[i].CheckNotCrossEnvironment())
                {
                    OnDiceReadyToAct?.Execute(DiceActions[i]);
                }
            }
        }

        public void SetUnReadyToAct()
        {
            OnDiceUnReadyToAct?.Execute();
        }

        public void UpdateActions(DiceAction[] diceAttributes)
        {
            if (diceAttributes.Length != FixedActionsAmount)
            {
                throw new ArgumentException("Actions amount is not designed for dice's condition");
            }

            DiceActions = diceAttributes;

            for (var i = 0; i < _diceEdges.Length; i++)
            {
                _diceEdges[i].UpdateAction(diceAttributes[i].DiceActionData);
            }
        }

        public void ActivatePhysicality()
        {
            Rigidbody.isKinematic = false;
            _collider.enabled = true;
        }

        public void DeactivatePhysicality()
        {
            Rigidbody.isKinematic = true;
            _collider.enabled = false;
        }
    }
}