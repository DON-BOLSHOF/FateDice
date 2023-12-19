using UnityEngine;

namespace BKA.Dices
{
    public class DiceEdge : MonoBehaviour
    {
        public bool CheckEnvironment()
        {
            var leftRay = Physics.SphereCast(transform.position, 0.25f, -transform.right, out _, 5,
                LayerMask.NameToLayer($"Environment"));
            var rightRay = Physics.SphereCast(transform.position, 0.25f, transform.right, out _, 5,
                LayerMask.NameToLayer($"Environment"));
            var middleRay = Physics.SphereCast(transform.position, 0.25f, transform.up, out _, 5,
                LayerMask.NameToLayer($"Environment"));

            return leftRay || rightRay || middleRay;
        }
    }
}