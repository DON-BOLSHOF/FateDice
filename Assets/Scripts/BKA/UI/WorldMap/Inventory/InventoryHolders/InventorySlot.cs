using System;
using BKA.WorldMapDirectory.Artefacts;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BKA.UI.WorldMap.Inventory
{
    public class InventorySlot : MonoBehaviour, IDropHandler
    {
        [SerializeField] private InventoryItem _inventoryItem;
        public InventoryItem InventoryItem => _inventoryItem;
        public IObservable<InventoryItem> OnDroppedItem => _onDroppedItem;

        public IObservable<Unit> OnUpdatedData => _inventoryItem.OnUpdatedData;

        public bool IsFullFilled => _inventoryItem.IsFullFilled;

        private ReactiveCommand<InventoryItem> _onDroppedItem = new();

        public void SetItem(Artefact item)
        {
            _inventoryItem.FullFill(item);
        }

        public void ClearSlot()
        {
            _inventoryItem.ClearData();
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (IsFullFilled)
            {
                return;
            }

            if (eventData.pointerDrag.TryGetComponent<InventoryItem>(out var inventoryItem))
            {
                _onDroppedItem?.Execute(inventoryItem);
            }
        }
    }
}