using System;
using BKA.Player;
using UniRx;
using UnityEngine;
using Zenject;

namespace BKA.WorldMapDirectory.Systems
{
    [RequireComponent(typeof(Collider2D))]
    public class InteractableObject : MonoBehaviour, IDisposable
    {
        [SerializeField] private Transform _hint;
        [SerializeField] private Animator _hintAnimator;

        [Inject] private PlayerInput _playerInput;

        public IObservable<Unit> OnInteracted => _onInteracted;

        private ReactiveCommand _onInteracted = new();
        private bool _isInteracted;
        private static readonly int IsInteracted = Animator.StringToHash("IsInteracted");

        private void Update()
        {
            if (_playerInput.GetInteractButton() && _isInteracted)
            {
                _onInteracted?.Execute();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<HeroComponent>())
            {
                _isInteracted = true;
                _hintAnimator.SetBool(IsInteracted, true);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.GetComponent<HeroComponent>())
            {
                _isInteracted = false;
                _hintAnimator.SetBool(IsInteracted, false);
            }
        }

        public void Dispose()
        {
            _onInteracted?.Dispose();
            _hint.gameObject.SetActive(false);
            enabled = false;
        }
    }
}