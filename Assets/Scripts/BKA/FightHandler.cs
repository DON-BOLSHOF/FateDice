using System;
using BKA.Units;
using UnityEngine;

namespace BKA
{
    public class FightHandler : MonoBehaviour
    {
        private UnitDefinition[] _firstPack;
        private UnitDefinition[] _secondPack;

        private void Start()
        {
        }

        public void DynamicInit(UnitDefinition[] teammates, UnitDefinition[] enemy)
        {
            _firstPack = teammates;
            _secondPack = enemy;
        }
    }
}
