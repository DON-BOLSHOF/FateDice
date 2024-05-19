using BKA.WorldMapDirectory.Systems;
using UniRx;
using UnityEngine;

namespace BKA.WorldMapDirectory.Quest
{
    [RequireComponent(typeof(InteractableObject))]
    public class InteractableQuestHolder : QuestHolder
    {
        private InteractableObject _interactableObject;
        
        protected override void Start()
        {
            base.Start();
            
            _interactableObject = GetComponent<InteractableObject>();

            _interactableObject.OnInteracted.Subscribe(_ => { TryActivateQuest(); }).AddTo(this);
        }

        public override void StartUpQuest()
        {
            base.StartUpQuest();
            _interactableObject.Dispose();
        }

        protected override void Activate()
        {
            GetComponent<InteractableObject>().enabled = true;
            enabled = true;
        }

        protected override void Deactivate()
        {
            GetComponent<InteractableObject>().Dispose();
            enabled = false;
        }
    }
}