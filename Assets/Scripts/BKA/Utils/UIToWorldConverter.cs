using UnityEngine;

namespace BKA.Utils
{
    public class UIToWorldConverter
    {
        public static Vector3 Convert(RectTransform rectTransform)
        {
            var temp = rectTransform.TransformPoint(Vector3.zero);
            temp.z =9.5f;

            return Camera.main.ScreenToWorldPoint(temp);
        }
    }
}