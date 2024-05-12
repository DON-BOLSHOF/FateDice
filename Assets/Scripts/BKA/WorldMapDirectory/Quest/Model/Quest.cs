﻿using System;
using UniRx;
using UnityEngine;
using Notification = BKA.UI.WorldMap.Notification;

namespace BKA.WorldMapDirectory.Quest
{
    public class Quest : INotifingObject, IDisposable
    {
        public readonly string QuestTitle;
        public IObservable<Unit> OnQuestCompleted => _onQuestCompleted;
        public readonly int XpForQuestCompleted;

        public IObservable<Notification> OnSentNotification => _onQuestSentNotification;
        
        private readonly QuestElement[] _questElements;
        private readonly ReactiveCommand _onQuestCompleted = new();
        private readonly ReactiveCommand<Notification> _onQuestSentNotification = new();

        private readonly CompositeDisposable _questDisposable = new();

        private int _currentElement;

        public Quest( string questTitle, QuestElement[] questElements, int xpForQuestCompleted)
        {
            QuestTitle = questTitle;
            _questElements = questElements;
            XpForQuestCompleted = xpForQuestCompleted;

            foreach (var questElement in _questElements)
            {
                questElement.OnElementCompleted.Subscribe(_ => ActivateNextElement()).AddTo(_questDisposable);
            }
        }

        public void ActivateQuest()
        {
            ActivateNextElement();
        }

        public void Dispose()
        {
            _onQuestCompleted?.Dispose();
            _questDisposable?.Dispose();
        }

        private void ActivateNextElement()
        {
            if (_currentElement < _questElements.Length)
            {
                _onQuestSentNotification?.Execute(new Notification(_questElements[_currentElement].QuestElementHint, QuestTitle));
                _questElements[_currentElement++].Activate();
            }
            else
            {
                _onQuestCompleted?.Execute();
            }
        }
    }
}