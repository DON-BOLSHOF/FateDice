using System;
using UniRx;
using UnityEngine;

namespace BKA.WorldMapDirectory.Quest
{
    public abstract class QuestElement : MonoBehaviour
    {
        public abstract void Activate();
        public abstract string QuestElementHint { get; }
        public IObservable<Unit> OnElementCompleted => _onElementCompleted;

        protected ReactiveCommand _onElementCompleted = new();
    }
}