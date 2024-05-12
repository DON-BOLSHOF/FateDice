using BKA.Player;
using UnityEngine;

namespace BKA.UI.WorldMap.Dialog
{
    [RequireComponent(typeof(Collider2D))]
    public class TriggerDialogPoint : DialogPoint
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<HeroComponent>(out var heroComponent))
            {
                ActivateDialog();
            }
        }
    }
}