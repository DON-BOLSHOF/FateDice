using System;
using BKA.WorldMapDirectory.Systems;
using UniRx;
using UnityEngine;

namespace BKA.WorldMapDirectory.Quest
{
    [RequireComponent(typeof(InteractableObject))]
    public class QuestHolder : MonoBehaviour
    {
        [SerializeField] private string _questTitle;
        [SerializeField] private QuestElement[] _questElementsSequence;
        [SerializeField] private int _xpForQuest;
        [SerializeField] private QuestInterlude _questInterlude;

        public Quest Quest => _quest;
        public QuestInterlude QuestInterlude => _questInterlude;
        public IObservable<Unit> OnTryQuestActivate => _onTryQuestActivate;

        private Quest _quest;

        private ReactiveCommand _onTryQuestActivate = new();
        private InteractableObject _interactableObject;
        
        private void Start()
        {
            _quest = new Quest(_questTitle,_questElementsSequence, _xpForQuest);
            
            _interactableObject = GetComponent<InteractableObject>();

            _interactableObject.OnInteracted.Subscribe(_ =>
            {
                _onTryQuestActivate.Execute();
            }).AddTo(this);
        }

        public void Deactivate()
        {
            enabled = false;
        }
    }
}