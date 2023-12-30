using BKA.Units;
using UnityEngine;
using UnityEngine.UI;

namespace BKA.UI
{
    public class CharacterPanel : MonoBehaviour
    {
        [SerializeField] private Image _portrait;
        [SerializeField] private Image _attribute;
        [SerializeField] private HealthWidget _healthWidget;

        public void Fulfill(UnitDefinition unitDefinition)
        {
            
        }
    }
}