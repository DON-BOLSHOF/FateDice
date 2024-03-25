using UnityEngine;

namespace BKA.UI
{
    public class HealthWidget : MonoBehaviour
    {
        [SerializeField] private GameObject[] _healthPoints;

        private int _healthValue;
        
        public void SetHealth(int value)
        {
            _healthValue = value;

            UpdateHealths(value);
        }

        private void UpdateHealths(int value)
        {
            for (var i = 0; i < _healthPoints.Length && i < value; i++)
            {
                _healthPoints[i].gameObject.SetActive(true);
            }

            for (int i = value; i < _healthPoints.Length; i++)
            {
                _healthPoints[i].gameObject.SetActive(false);
            }
        }
    }
}