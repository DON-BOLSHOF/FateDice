using System;
using System.Collections.Generic;
using System.Linq;
using BKA.Buffs;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace BKA.UI.WorldMap.Inventory
{
    public class ItemHolder : MonoBehaviour
    {
        [SerializeField] protected InventorySlot[] _inventorySlots;

        public IObservable<Unit> OnUpdatedData => _onUpdatedData;

        private ReactiveCommand _onUpdatedData = new();

        protected virtual void Awake()
        {
            foreach (var inventorySlot in _inventorySlots)
            {
                inventorySlot.OnDroppedItem.Subscribe(inventoryItem => ReloadSlots(inventorySlot, inventoryItem))
                    .AddTo(this);
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

            for (; index < _inventorySlots.Length; index++)
            {
                _inventorySlots[index].ClearSlot();
            }
        }

        public void UploadArtefact(Artefact artefact)
        {
            foreach (var inventorySlot in _inventorySlots)
            {
                if (inventorySlot.IsFullFilled) continue;
                
                inventorySlot.SetItem(artefact);
                return;
            }
        }
        
        public void ClearArtefact(Artefact artefact)
        {
            foreach (var inventorySlot in _inventorySlots)
            {
                if (!inventorySlot.IsFullFilled || !inventorySlot.InventoryItem.Artefact.Equals(artefact)) continue;
                
                inventorySlot.ClearSlot();
                return;
            }
        }

        public List<Artefact> GetArtefacts()
        {
            var result = (from inventorySlot in _inventorySlots
                where inventorySlot.IsFullFilled
                select inventorySlot.InventoryItem.Artefact).ToList();

            return result;
        }

        private void ReloadSlots(InventorySlot inventorySlotTo, InventoryItem inventoryItemFrom)
        {
            var artefact = inventoryItemFrom.Artefact;
            
            inventoryItemFrom.ClearData();

            inventorySlotTo.SetItem(artefact);

            _onUpdatedData?.Execute();
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