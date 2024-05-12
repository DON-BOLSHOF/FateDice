using System;
using System.Collections.Generic;
using BKA.UI.WorldMap.Quest.Interfaces;
using BKA.Zenject.Signals;
using Cysharp.Threading.Tasks;
using UniRx;
using Zenject;

namespace BKA.WorldMapDirectory.Quest
{
    public class QuestHandler : IDisposable
    {
        private List<Quest> _activatedQuest = new();

        private IQuestPanel _questPanel;

        private INotificationHandler _notificationHandler;

        private SignalBus _signalBus;
        private CompositeDisposable _handlerDisposable = new();

        public QuestHandler(QuestHolder[] questHolder, IQuestPanel questPanel, INotificationHandler notificationHandler,SignalBus signalBus)
        {
            _questPanel = questPanel;
            _signalBus = signalBus;
            _notificationHandler = notificationHandler;
            
            foreach (var holder in questHolder)
            {
                holder.OnTryQuestActivate.Subscribe(_ => TryActivateQuest(holder).Forget()).AddTo(_handlerDisposable);
            }
        }

        public void Dispose()
        {
            foreach (var quest in _activatedQuest)
            {
                quest.Dispose();
            }

            _handlerDisposable?.Dispose();
        }

        private async UniTaskVoid TryActivateQuest(QuestHolder questHolder)
        {
            _signalBus.Fire( new BlockInputSignal{IsBlocked = true});
            
            _questPanel.ActivatePanel(questHolder.QuestInterlude);

            var isActivated = await _questPanel.OnActivateQuest.ToUniTask(useFirstValue: true);

            if (isActivated)
            {
                var quest = questHolder.Quest;
                _activatedQuest.Add(quest);
                
                _notificationHandler.LoadNotificationObject(quest);

                quest.OnQuestCompleted.Subscribe(_ => OnQuestCompleted(quest)).AddTo(_handlerDisposable);
                quest.ActivateQuest();
                
                questHolder.Deactivate();
            }
            _signalBus.Fire( new BlockInputSignal{IsBlocked = false});
        }

        private void OnQuestCompleted(Quest quest)
        {
            _signalBus.Fire(new GiveXPSignal{XP = quest.XpForQuestCompleted});
            
            _notificationHandler.RemoveNotificationObject(quest);
            
            _activatedQuest.Remove(quest);
            quest.Dispose();
        }
    }
}