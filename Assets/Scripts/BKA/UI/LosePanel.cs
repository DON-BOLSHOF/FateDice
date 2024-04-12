using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace BKA.UI
{
    public class LosePanel : MonoBehaviour
    {
        [SerializeField] private Button _loseButton;

        public IObservable<Unit> OnLoseClicked => _onLoseClicked;

        private ReactiveCommand _onLoseClicked = new();
        
        private void Start()
        {
            _loseButton.OnClickAsObservable().Subscribe(_ => _onLoseClicked?.Execute()).AddTo(this);
        }
    }
}