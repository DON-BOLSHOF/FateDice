using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BKA
{
    public class TestRand : MonoBehaviour
    {
        private void Start()
        {
            var rand = Random.Range(5, 5);
            
            Debug.Log(rand);
        }
    }
}