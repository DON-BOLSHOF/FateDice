using UnityEngine;

namespace BKA.WorldMapDirectory
{
    public class HeroPositionComponent : MonoBehaviour
    {
        public void DynamicInit(Vector3 position)
        {
            transform.position = position;
        }
    }
}