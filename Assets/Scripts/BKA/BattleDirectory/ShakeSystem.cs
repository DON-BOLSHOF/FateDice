using System.Collections.Generic;
using BKA.Dices;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BKA.BattleDirectory
{
    public class ShakeSystem : MonoBehaviour
    {
        public void ShakeObjects(IEnumerable<DiceObject> diceObjects)
        {
            foreach (var diceObject in diceObjects)
            {
                ShakeObject(diceObject);
            }
        }

        private void ShakeObject(DiceObject diceObject)
        {
            diceObject.Rigidbody.AddForce(new Vector3(GetRandom(50, 120), Random.Range(200, 350), GetRandom(25, 50)));
            diceObject.Rigidbody.AddTorque(new Vector3(GetRandom(25, 50), GetRandom(25, 50), GetRandom(25, 50)));
        }

        private int GetRandom(int minimum, int maximum)
        {
            var value = Random.Range(minimum, maximum);

            var isPositive = Random.Range(0, 2) > 0;

            return isPositive ? value : -value;
        }
    }
}