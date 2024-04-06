using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace BKA.UI
{
    [RequireComponent(typeof(Button))]
    public class ReadyWidget : MonoBehaviour
    {
        public IObservable<Unit> OnReady => _onReady;
        
        private ReactiveCommand _onReady = new();
        private Button _button;

        private void Start()
        {
            _button = GetComponent<Button>();

            _button.OnClickAsObservable().Subscribe(_ => _onReady?.Execute()).AddTo(this);
        }
    }
}