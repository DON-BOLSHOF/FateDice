using System;
using BKA.Buffs;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BKA.UI.WorldMap.Inventory
{
    [RequireComponent(typeof(Image))]
    public class InventoryItem : DraggableItem
    {
        [SerializeField] private Image _image;
        public Artefact Artefact => _artefact;
        public bool IsFullFilled => _artefact != null;

        public IObservable<Unit> OnUpdatedData => _onUpdatedData;

        private ReactiveCommand _onUpdatedData = new();
        private Artefact _artefact;

        public virtual void FullFill(Artefact artefact)
        {
            _artefact = artefact;
            _image.sprite = artefact.View;

            _onUpdatedData?.Execute();
            
            ActivateBlockRaycast(artefact.StateOfArtefact == ArtefactState.Buff);
        }

        public virtual void ClearData()
        {
            _artefact = null;
            _image.sprite = null;
            
            _onUpdatedData?.Execute();
            
            ActivateBlockRaycast(false);
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            
            ActivateBlockRaycast(IsFullFilled);
        }
    }
}