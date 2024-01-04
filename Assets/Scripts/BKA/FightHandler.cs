using System;
using BKA.Units;
using UnityEngine;

namespace BKA
{
    public class FightHandler : MonoBehaviour
    {
        private Unit[] _firstPack;
        private Unit[] _secondPack;

        private void Start()
        {
        }

        public void DynamicInit(Unit[] teammates, Unit[] enemy)
        {
            _firstPack = teammates;
            _secondPack = enemy;
        }
    }
}
