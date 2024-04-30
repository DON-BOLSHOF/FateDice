using UnityEngine;

namespace BKA.Buffs
{
    [CreateAssetMenu(menuName = "Defs/Specialization/SpecializationSequence", fileName = "SpecializationSequence")]
    public class SpecializationSequence : ScriptableObject
    {
        [SerializeField] private SpecializationDefinition _currentSpecialization;
        [SerializeField] private SpecializationDefinition[] _toSpecializations;

        public SpecializationDefinition CurrentSpecialization => _currentSpecialization;
        public SpecializationDefinition[] ToSpecializations => _toSpecializations;
    }
}