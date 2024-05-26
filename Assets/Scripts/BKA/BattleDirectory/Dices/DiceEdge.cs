using BKA.Dices.DiceActions;
using TMPro;
using UnityEngine;

namespace BKA.Dices
{
    public class DiceEdge : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _edgeView;

        [SerializeField] private Transform _modificatorBackground;
        [SerializeField] private TextMeshProUGUI _modificatorValue;

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

        public void UpdateAction(DiceAction action)
        {
            _edgeView.sprite = action.DiceActionData.ActionView;
            var modificator = action.ActionModificatorValue;
            if (modificator > 0)
            {
                _modificatorValue.text = modificator.ToString();
                _modificatorValue.gameObject.SetActive(true);
                _modificatorBackground.gameObject.SetActive(true);
            }
            else
            {
                _modificatorBackground.gameObject.SetActive(false);
                _modificatorValue.gameObject.SetActive(false);
            }
        }
    }
}