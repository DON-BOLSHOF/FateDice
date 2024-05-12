using System;
using BKA.WorldMapDirectory.Systems;
using UniRx;
using UnityEngine;

namespace BKA.WorldMapDirectory.Quest
{
    [Serializable]
    public class QuestHolderData
    {
        public bool IsTaken;
    }
    
    [RequireComponent(typeof(InteractableObject))]
    public class QuestHolder : MonoBehaviour
    {
        [SerializeField] private string _questTitle;
        [SerializeField] private QuestElement[] _questElementsSequence;
        [SerializeField] private int _xpForQuest;
        [SerializeField] private QuestInterlude _questInterlude;

        public Quest Quest => _quest ?? new Quest(_questTitle,_questElementsSequence, _xpForQuest);
        public QuestHolderData QuestHolderData => _questHolderData;
        public QuestInterlude QuestInterlude => _questInterlude;
        public IObservable<Unit> OnTryQuestActivate => _onTryQuestActivate;
        
        public string QuestTitle => _questTitle;

        private Quest _quest;

        private QuestHolderData _questHolderData = new();

        private ReactiveCommand _onTryQuestActivate = new();
        private InteractableObject _interactableObject;
        
        private void Start()
        {
            _quest ??= new Quest(_questTitle,_questElementsSequence, _xpForQuest);
            
            _interactableObject = GetComponent<InteractableObject>();

            _interactableObject.OnInteracted.Subscribe(_ =>
            {
                _questHolderData.IsTaken = true;
                _onTryQuestActivate.Execute();
            }).AddTo(this);
        }

        public void Deactivate()
        {
            enabled = false;
            _interactableObject.Dispose();
        }

        public void DynamicInit(QuestHolderData questHolderData)
        {
            _questHolderData = questHolderData;
            
            if (_questHolderData.IsTaken)
            {
                enabled = false;
                GetComponent<InteractableObject>().Dispose();
            }
        }
    }
}