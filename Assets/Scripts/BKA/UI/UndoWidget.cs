using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace BKA.UI
{
    [RequireComponent(typeof(Button))]
    public class UndoWidget : MonoBehaviour
    {
        public IObservable<Unit> OnUndo => _onUndo;
        
        private ReactiveCommand _onUndo = new();
        private Button _button;

        private void Start()
        {
            _button = GetComponent<Button>();

            _button.OnClickAsObservable().Subscribe(_ => _onUndo?.Execute()).AddTo(this);
        }
    }
}