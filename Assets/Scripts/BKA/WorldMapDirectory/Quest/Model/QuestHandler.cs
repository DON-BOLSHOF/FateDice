using System;
using System.Collections.Generic;
using System.Linq;
using BKA.UI.WorldMap.Quest.Interfaces;
using BKA.Zenject.Signals;
using Cysharp.Threading.Tasks;
using UniRx;
using Zenject;

namespace BKA.WorldMapDirectory.Quest
{
    public class QuestHandler : IQuestHandler,IDisposable
    {
        public IEnumerable<Quest> ActivatedQuests => _activatedQuest;
        
        private List<Quest> _activatedQuest = new();

        private IQuestPanel _questPanel;
        private QuestHolder[] _questHolders;

        private INotificationHandler _notificationHandler;

        private SignalBus _signalBus;
        private CompositeDisposable _handlerDisposable = new();

        public QuestHandler(QuestHolder[] questHolder, IQuestPanel questPanel, INotificationHandler notificationHandler,SignalBus signalBus)
        {
            _questPanel = questPanel;
            _signalBus = signalBus;
            _notificationHandler = notificationHandler;
            _questHolders = questHolder;
            
            foreach (var holder in _questHolders)
            {
                holder.OnTryQuestActivate.Subscribe(_ => TryActivateQuest(holder).Forget()).AddTo(_handlerDisposable);
            }
        }

        public void UploadActivatedQuests(IEnumerable<QuestData> questsData)
        {
            foreach (var questData in questsData)
            {
                var holder = _questHolders.First(holder => holder.QuestTitle.Equals(questData.QuestTitle));
                var quest = holder.Quest;
                
                _activatedQuest.Add(quest);
                
                _notificationHandler.LoadNotificationObject(quest);

                quest.OnQuestCompleted.Subscribe(_ => OnQuestCompleted(quest)).AddTo(_handlerDisposable);
                
                quest.ForceMoveSteps(questData.CurrentElement);
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
                quest.StartUpQuest();
                
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