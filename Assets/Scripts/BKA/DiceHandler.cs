using System;
using System.Collections.Generic;
using BKA.Dices;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace BKA
{
    public class DiceHandler : MonoBehaviour
    {
        [SerializeField] private ShakeSystem _shakeSystem;

        private List<DiceObject> _activeDices = new();

        private List<DiceObject> _inactiveDices = new();

        private List<DiceObject> _dicePool = new();

        [Inject] private DiceFactory _diceFactory;

        public List<DiceObject> UploadNewDices(int unitsLenght)
        {
            var result = new List<DiceObject>();

            for (int i = 0; i < unitsLenght; i++)
            {
                result.Add(_diceFactory.CreateCubeDice(transform, new Vector3(Random.Range(-3,3),8,Random.Range(-3,3))));
            }

            _dicePool.AddRange(result);
            
            return result;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _shakeSystem.ShakeObjects(_dicePool);
            }
        }
    }
}