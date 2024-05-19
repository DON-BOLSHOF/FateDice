using System;
using UniRx;
using UnityEngine;

namespace BKA.UI
{
    public class DisclaimerPanel : MonoBehaviour
    {
        public IObservable<Unit> OnEnded => _onEnded;
        private ReactiveCommand _onEnded = new();
        
        public void OnDisclaimerEnded()
        {
            _onEnded?.Execute();
        }
    }
}