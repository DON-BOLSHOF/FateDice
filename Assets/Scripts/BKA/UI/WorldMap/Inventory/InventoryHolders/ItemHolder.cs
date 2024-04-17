using System;
using System.Collections.Generic;
using System.Linq;
using BKA.WorldMapDirectory.Artefacts;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace BKA.UI.WorldMap.Inventory
{
    public class ItemHolder : MonoBehaviour
    {
        [SerializeField]
        protected InventorySlot[] _inventorySlots;

        protected virtual void Awake()
        {
            foreach (var inventorySlot in _inventorySlots)
            {
                inventorySlot.OnDroppedItem.Subscribe(inventoryItem => ReloadSlots(inventorySlot, inventoryItem)).AddTo(this);
            }
        }

        public void SetArtefacts(List<Artefact> partyArtefacts)
        {
            if (partyArtefacts.Count > _inventorySlots.Length)
                throw new ArgumentException("Too many artefacts");

            int index = 0;
            for (index = 0; index < partyArtefacts.Count; index++)
            {
                _inventorySlots[index].SetItem(partyArtefacts[index]);
            }
            
            for (;index < _inventorySlots.Length; index++)
            {
                _inventorySlots[index].ClearSlot();
            }
        }

        protected virtual void ReloadSlots(InventorySlot inventorySlotTo, InventoryItem inventoryItemFrom)
        {
            inventorySlotTo.SetItem(inventoryItemFrom.Artefact);
            
            inventoryItemFrom.ClearData();
        }
        
        #if UNITY_EDITOR
        [Button]
        private void GetSlots()
        {
            _inventorySlots = GetComponentsInChildren<InventorySlot>();
        }
        #endif
    }
}