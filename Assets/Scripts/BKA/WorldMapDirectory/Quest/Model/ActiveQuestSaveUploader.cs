using System;
using System.Collections.Generic;
using System.Linq;
using BKA.System.UploadData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BKA.WorldMapDirectory.Quest
{
    public class LocalActiveQuestData
    {
        public List<QuestData> QuestDatas;
    }
    
    public class ActiveQuestSaveUploader : ISaveUploader, IDisposable
    {
        private IQuestHandler _questHandler;
        
        private LocalActiveQuestData _localActiveQuestData = new();
        private const string _SAVE_CODE = "ACTIVE_QUESTS_DATA";
        
        public ActiveQuestSaveUploader(IQuestHandler questHandler)
        {
            _questHandler = questHandler;
        }

        public void UploadBaseSaves()
        {
            //Допилить
        }

        public async UniTask UploadLocalSaves()
        {
            if (TryGetSaves())
            {
                _questHandler.UploadActivatedQuests(_localActiveQuestData.QuestDatas);

                await UniTask.Delay(TimeSpan.FromMilliseconds(15));
            }
            else
            {
                throw new ApplicationException("Ошибка локального сохранения");
            }
        }

        public void Dispose()
        {
            _localActiveQuestData.QuestDatas = _questHandler.ActivatedQuests.Select(quest => quest.GetSerializableData()).ToList();

            var save = JsonUtility.ToJson(_localActiveQuestData);
            PlayerPrefs.SetString(_SAVE_CODE, save);
        }

        private bool TryGetSaves()
        {
            var json = PlayerPrefs.GetString(_SAVE_CODE);
            var instance = JsonUtility.FromJson<LocalActiveQuestData>(json);

            if (instance == null) return false;

            _localActiveQuestData = instance;

            return true;
        }
    }
}