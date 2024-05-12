using System.Collections.Generic;

namespace BKA.WorldMapDirectory.Quest
{
    public interface IQuestHandler
    {
        IEnumerable<Quest> ActivatedQuests { get; }
        void UploadActivatedQuests(IEnumerable<QuestData> questsData);
    }
}