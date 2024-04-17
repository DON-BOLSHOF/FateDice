using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace BKA.UI.Inventory
{
    public class InventoryButton : MonoBehaviour
    {
        [SerializeField] private Button _inventoryButton;

        public IObservable<Unit> OnInventoryWidgetClicked=> _onInventoryWidgetClicked;

        private ReactiveCommand _onInventoryWidgetClicked = new();
        
        private void Start()
        {
            _inventoryButton.OnClickAsObservable().Subscribe(_ => _onInventoryWidgetClicked?.Execute()).AddTo(this);
        }
    }
}