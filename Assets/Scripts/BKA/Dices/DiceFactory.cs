using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BKA.Dices
{
    [Serializable]
    public class DiceFactory
    {
        [SerializeField] private CubeDice _cubeDicePrefab;

        public CubeDice CreateCubeDice(Transform parent = null, Vector3 position = default)
        {
            var cubeDice = Object.Instantiate(_cubeDicePrefab, position, Quaternion.identity, parent);

            return cubeDice;
        }
    }
}