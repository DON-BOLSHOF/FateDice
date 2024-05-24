using System;
using UniRx;
using Notification = BKA.UI.WorldMap.Notification;

namespace BKA.WorldMapDirectory.Quest
{
    [Serializable]
    public class QuestData
    {
        public string QuestTitle;
        public int XpFOrQuestCompleted;
        public int CurrentElement;
    }

    public class Quest : INotifingObject, IDisposable
    {
        public readonly string QuestTitle;
        public IObservable<Unit> OnQuestCompleted => _onQuestCompleted;
        public readonly int XpForQuestCompleted;

        public readonly bool IsMainQuest;

        public IObservable<Notification> OnSentNotification => _onQuestSentNotification;

        private readonly QuestElement[] _questElements;
        private readonly ReactiveCommand _onQuestCompleted = new();
        private readonly ReactiveCommand<Notification> _onQuestSentNotification = new();
        
        private readonly CompositeDisposable _questDisposable = new();

        private int _currentElement;

        public Quest(string questTitle, QuestElement[] questElements, int xpForQuestCompleted, bool isMainQuest)
        {
            QuestTitle = questTitle;
            _questElements = questElements;
            XpForQuestCompleted = xpForQuestCompleted;
            IsMainQuest = isMainQuest;

            foreach (var questElement in _questElements)
            {
                questElement.OnElementCompleted.Subscribe(_ => ActivateNextElement()).AddTo(_questDisposable);
            }
        }

        public void StartUpQuest()
        {
            _currentElement = 0;
            
            _onQuestSentNotification?.Execute(new Notification(_questElements[_currentElement].QuestElementHint,
                QuestTitle));
            _questElements[_currentElement].Activate();
        }

        public void ForceMoveSteps(int step)
        {
            _currentElement = step;
            _questElements[_currentElement].Activate();
        }

        public QuestData GetSerializableData()
        {
            return new QuestData
            {
                QuestTitle = QuestTitle, XpFOrQuestCompleted = XpForQuestCompleted, CurrentElement = _currentElement
            };
        }

        public void Dispose()
        {
            _onQuestCompleted?.Dispose();
            _questDisposable?.Dispose();
        }

        private void ActivateNextElement()
        {
            _currentElement++;
            
            if (_currentElement < _questElements.Length)
            {
                _onQuestSentNotification?.Execute(new Notification(_questElements[_currentElement].QuestElementHint,
                    QuestTitle));
                _questElements[_currentElement].Activate();
            }
            else
            {
                _onQuestCompleted?.Execute();
            }
        }
    }
}