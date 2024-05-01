using System;
using UnityEngine;

namespace BKA.Units
{
    [Serializable]
    public class Characteristics
    {
        [SerializeField] private int _agility;
        [SerializeField] private int _strength;
        [SerializeField] private int _intelligent;

        public int Agility => _agility;
        public int Strength => _strength;
        public int Intelligent => _intelligent;

        public Characteristics(int agility, int strength, int intelligent)
        {
            _agility = agility;
            _strength = strength;
            _intelligent = intelligent;
        }

        public void ModifyCharacteristics(Characteristics characteristics)
        {
            _agility += characteristics._agility;
            _strength += characteristics._strength;
            _intelligent += characteristics._intelligent;
        }

        public void FullUpdateData(Characteristics characteristics)
        {
            _agility = characteristics._agility;
            _strength = characteristics._strength;
            _intelligent = characteristics._intelligent;
        }

        public Characteristics Clone()
        {
            return new Characteristics(_agility, _strength, _intelligent);
        }
    }
}