using System;
using UniRx;
using UnityEngine;

namespace BKA.WorldMapDirectory.Systems
{
    [RequireComponent(typeof(Collider2D))]
    public class InteractableObject : MonoBehaviour
    {
        public IObservable<Unit> OnInteracted => _onInteracted;

        private ReactiveCommand _onInteracted = new();
        private bool _isInteracted;

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.E) && _isInteracted)
            {
                _onInteracted?.Execute();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            _isInteracted = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            _isInteracted = false;
        }
    }
}