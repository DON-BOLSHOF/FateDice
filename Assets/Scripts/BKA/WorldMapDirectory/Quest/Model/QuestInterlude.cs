using System;
using UnityEngine;

namespace BKA.WorldMapDirectory.Quest
{
    [Serializable]
    public class QuestInterlude
    {
        [SerializeField] private string _questName;
        [SerializeField] private string _description;

        [SerializeField] private bool _isAdditionalQuest;

        [SerializeField] private string _activationButtonDescription;

        public string QuestName => _questName;
        public string Description => _description;
        public bool IsAdditionalQuest => _isAdditionalQuest;
        public string ActivationButtonDescription => _activationButtonDescription;
    }
}