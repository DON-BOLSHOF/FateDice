using System;
using BKA.Dices.Attributes;
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
        protected abstract int FixedActionsAmount { get; }
        protected abstract DiceEdge[] _diceEdges { get; set;}

        private Collider _collider;

        public ReactiveCommand<DiceAction> OnDiceSelected = new();

        private void Start()
        {
            Rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();

            if (FixedActionsAmount != _diceEdges.Length)
            {
                throw new ArgumentException("Edges amount is not equal fixed count!");
            }
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
                _diceEdges[i].UpdateAction(diceAttributes[i]);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            for (int i = 0; i < _diceEdges.Length; i++)
            {
                if (_diceEdges[i].CheckNotCrossEnvironment())
                {
                    Debug.Log(_diceEdges[i].name);    
                    
                    OnDiceSelected?.Execute(DiceActions[i]);
                }
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