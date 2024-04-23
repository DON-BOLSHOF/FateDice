using System;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace BKA.UI.WorldMap.Class
{
    public class ClassButton : MonoBehaviour
    {
        [SerializeField] private Button _classButton;

        public IObservable<Unit> OnClassButtonClicked => _onClassButtonClicked;

        private ReactiveCommand _onClassButtonClicked = new();

        private void Start()
        {
            _classButton.OnClickAsObservable().Subscribe(_ => _onClassButtonClicked?.Execute()).AddTo(this);
        }
    }
}