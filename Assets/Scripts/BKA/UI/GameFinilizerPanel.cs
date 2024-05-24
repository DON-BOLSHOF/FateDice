using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace BKA.UI
{
    public class GameFinilizerPanel : MonoBehaviour
    {
        [SerializeField] private Transform _view;
        [SerializeField] private Button _finishButton;
        
        public IObservable<Unit> OnFinishedGame => _onFinishedGame;

        private ReactiveCommand _onFinishedGame = new();

        private void Start()
        {
            _finishButton.OnClickAsObservable().Subscribe(_ => _onFinishedGame?.Execute()).AddTo(this);
        }

        public void Activate(bool value)
        {
            _view.gameObject.SetActive(value);
        }
    }
}