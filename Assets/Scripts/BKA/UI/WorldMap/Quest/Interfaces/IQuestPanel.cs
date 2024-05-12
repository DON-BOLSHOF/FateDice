using System;
using BKA.WorldMapDirectory.Quest;
using UniRx;

namespace BKA.UI.WorldMap.Quest.Interfaces
{
    public interface IQuestPanel
    {
        IObservable<bool> OnActivateQuest { get; }

        void ActivatePanel(QuestInterlude interlude);
    }
}