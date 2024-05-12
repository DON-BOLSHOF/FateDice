using UnityEngine;

namespace BKA.Player
{
    public class PlayerInput
    {
        private bool _isBlocked;

        public void Block(bool value)
        {
            _isBlocked = value;
        }
        
        public bool GetInteractButton()
        {
            return !_isBlocked && Input.GetKeyUp(KeyCode.E);
        }
    }
}