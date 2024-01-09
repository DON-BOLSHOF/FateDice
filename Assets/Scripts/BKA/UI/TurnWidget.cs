using UnityEngine;
using Zenject;

namespace BKA.UI
{
    public class TurnWidget : MonoBehaviour
    {
        [Inject] private TurnSystem _turnSystem;

        private void NextTurn()
        {
            _turnSystem.NextTurn();
        }
    }
}