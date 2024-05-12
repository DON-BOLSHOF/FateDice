using UniRx;
using UnityEngine;

namespace BKA.WorldMapDirectory.Systems
{
    [RequireComponent(typeof(InteractableObject))]
    public class InteractableBattlePoint : BattlePoint
    {
        private InteractableObject _interactableObject;
        
        private void Start()
        {
            _interactableObject = GetComponent<InteractableObject>();
            
            _interactableObject.OnInteracted.Subscribe(_ => StartBattle()).AddTo(_pointDisposable);
        }

        public override void Dispose()
        {
            GetComponent<InteractableObject>().Dispose();
            _onBattleStart?.Dispose();
            _pointDisposable?.Dispose();
        }
    }
}