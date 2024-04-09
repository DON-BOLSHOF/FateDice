using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace BKA.UI
{
    [RequireComponent(typeof(Button))]
    public class CharacterStateWidget : MonoBehaviour
    {
        public IObservable<Unit> OnClicked => _onStateButtonClicked;

        public Button StateButton => _stateButton;
        
        private Button _stateButton;

        private readonly ReactiveCommand _onStateButtonClicked = new();
        
        private void Awake()
        {
            _stateButton = GetComponent<Button>();

            _stateButton.OnClickAsObservable().Subscribe(_ => _onStateButtonClicked.Execute()).AddTo(this);
        }
    }
}