using System.Collections;
using UnityEngine;

namespace BKA.Utils.CodeAnimation
{
    public static class RaiseAnimation
    {
        public static IEnumerator RaiseRectTransformObject(RectTransform rectTransform, Vector3 raisePoint)
        {
            var currentPosition = rectTransform.anchoredPosition;
            for (float i = 0; i <= 1; i += 0.075f)
            {
                rectTransform.anchoredPosition = Vector3.Lerp(currentPosition, raisePoint, i);
                yield return new WaitForSeconds(0.035f);
            }
        }
    }
}