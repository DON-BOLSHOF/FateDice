using BKA.Player;
using UnityEngine;

namespace BKA.WorldMapDirectory.Quest
{
    [RequireComponent(typeof(Collider2D))]
    public class TriggerQuestHolder : QuestHolder
    {
        [SerializeField] private Collider2D _collider;

        public override void StartUpQuest()
        {
            base.StartUpQuest();

            _collider.enabled = false;
        }

        protected override void Activate()
        {
            _collider.enabled = true;
            enabled = true;
        }

        protected override void Deactivate()
        {
            _collider.enabled = false;
            enabled = false;
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.TryGetComponent<HeroComponent>(out _))
            {
                TryActivateQuest();
            }
        }
    }
}