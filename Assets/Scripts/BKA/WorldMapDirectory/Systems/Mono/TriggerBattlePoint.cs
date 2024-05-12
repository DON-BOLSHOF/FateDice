using BKA.Player;
using UnityEngine;

namespace BKA.WorldMapDirectory.Systems
{
    [RequireComponent(typeof(Collider2D))]
    public class TriggerBattlePoint : BattlePoint
    {
        private void OnTriggerEnter2D(Collider2D collider2D)
        {
            if (collider2D.GetComponent<HeroComponent>())
            {
                StartBattle();
            }
        }

        public override void Dispose()
        {
            _onBattleStart?.Dispose();
            _pointDisposable?.Dispose();
        }
    }
}