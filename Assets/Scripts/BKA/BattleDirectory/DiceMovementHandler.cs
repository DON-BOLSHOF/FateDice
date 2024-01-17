using UniRx;
using UnityEngine;

namespace BKA
{
    public class DiceMovementHandler : MonoBehaviour
    {
        private ReactiveProperty<bool> _isMovementComplete = new(true);
        public IReadOnlyReactiveProperty<bool> IsMovementComplete => _isMovementComplete;
    }
}