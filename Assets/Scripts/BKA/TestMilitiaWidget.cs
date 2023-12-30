using System;
using BKA.Units;
using UnityEngine;

namespace BKA
{
    public class TestMilitiaWidget : MonoBehaviour
    {
        private Unit _unit;
        
        private void Start()
        {
            _unit = new Militia();
        }
    }
}