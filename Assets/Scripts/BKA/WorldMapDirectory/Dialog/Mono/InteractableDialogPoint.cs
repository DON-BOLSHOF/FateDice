using BKA.WorldMapDirectory.Systems;
using UniRx;
using UnityEngine;

namespace BKA.UI.WorldMap.Dialog
{
    [RequireComponent(typeof(InteractableObject))]
    public class InteractableDialogPoint : DialogPoint
    {
        private InteractableObject _interactableObject;
        
        protected override void Start()
        {
            base.Start();

            _interactableObject = GetComponent<InteractableObject>();

            _interactableObject.OnInteracted.Subscribe(_ => ActivateDialog()).AddTo(this);
        }
    }
}