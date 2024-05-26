using System;
using System.Collections.Generic;
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
        
        public List<DiceObject> UploadNewDices(int unitsLenght, Transform transform = null, Vector3 position = default)
        {
            var result = new List<DiceObject>();

            for (int i = 0; i < unitsLenght; i++)
            {
                result.Add(CreateCubeDice(transform, position));
            }

            return result;
        }
    }
}