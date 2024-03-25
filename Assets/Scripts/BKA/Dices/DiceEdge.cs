using BKA.Dices.DiceActions;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace BKA.Dices
{
    public class DiceEdge : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _edgeView;
        
        public bool CheckNotCrossEnvironment()
        {
            return !CheckCrossEnvironment();
        }

        public bool CheckCrossEnvironment()
        {
            var leftRay = Physics.Raycast(transform.position, -transform.right + transform.forward, out _, 5,
                LayerMask.GetMask("Environment"));
            Debug.DrawRay(transform.position, -transform.right + transform.forward, Color.blue, 0.5f);

            var upRay = Physics.Raycast(transform.position, transform.forward + transform.up, out _, 5,
                LayerMask.GetMask("Environment"));
            Debug.DrawRay(transform.position, transform.forward + transform.up, Color.blue, 0.5f);

            var downRay = Physics.Raycast(transform.position, transform.forward - transform.up, out _, 5,
                LayerMask.GetMask("Environment"));
            Debug.DrawRay(transform.position, transform.forward - transform.up, Color.blue, 0.5f);

            var rightRay = Physics.Raycast(transform.position, transform.right + transform.forward, out _, 5,
                LayerMask.GetMask("Environment"));
            Debug.DrawRay(transform.position, transform.right + transform.forward, Color.blue, 0.5f);

            var middleRay = Physics.Raycast(transform.position, transform.forward, out _, 5,
                LayerMask.GetMask("Environment"));
            Debug.DrawRay(transform.position, transform.forward, Color.blue, 0.5f);

            return leftRay || rightRay || middleRay || upRay || downRay;
        }

        public void UpdateAction(DiceActionData diceAttribute)
        {
            _edgeView.sprite = diceAttribute.ActionView;
        }
    }
}