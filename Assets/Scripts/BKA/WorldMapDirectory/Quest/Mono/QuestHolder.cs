using System;
using UniRx;
using UnityEngine;

namespace BKA.WorldMapDirectory.Quest
{
    [Serializable]
    public class QuestHolderData
    {
        public bool IsTaken;
    }

    public abstract class QuestHolder : MonoBehaviour
    {
        [SerializeField] private string _questTitle;
        [SerializeField] private QuestElement[] _questElementsSequence;
        [SerializeField] private int _xpForQuest;
        [SerializeField] private QuestInterlude _questInterlude;
        [SerializeField] private bool _isMainQuest;

        public Quest Quest => _quest ?? new Quest(_questTitle, _questElementsSequence, _xpForQuest, _isMainQuest);
        public QuestHolderData QuestHolderData => _questHolderData;
        public QuestInterlude QuestInterlude => _questInterlude;
        public IObservable<Unit> OnTryQuestActivate => _onTryQuestActivate;

        public string QuestTitle => _questTitle;

        public bool IsMainQuest => _isMainQuest;

        private Quest _quest;

        private QuestHolderData _questHolderData;

        private ReactiveCommand _onTryQuestActivate = new();

        protected virtual void Start()
        {
            _quest ??= new Quest(_questTitle, _questElementsSequence, _xpForQuest, _isMainQuest);
        }

        public virtual void StartUpQuest()
        {
            enabled = false;
            _questHolderData.IsTaken = true;
        }

        public void DynamicInit(QuestHolderData questHolderData)
        {
            _questHolderData = questHolderData;

            if (!_questHolderData.IsTaken)
                Activate();
            else
                Deactivate();
        }

        protected abstract void Activate();
        protected abstract void Deactivate();

        protected void TryActivateQuest()
        {
            _onTryQuestActivate.Execute();
        }
    }
}