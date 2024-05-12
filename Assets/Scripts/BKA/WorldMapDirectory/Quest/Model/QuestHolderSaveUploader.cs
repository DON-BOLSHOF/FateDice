using System;
using System.Collections.Generic;
using System.Linq;
using BKA.System.UploadData;
using BKA.UI.WorldMap.Dialog;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BKA.WorldMapDirectory.Quest
{
    public class LocalQuestHolderData
    {
        public List<QuestHolderData> QuestHolderDatas;
    }
    
    public class QuestHolderSaveUploader : ISaveUploader, IDisposable
    {
        private QuestHolder[] _questHolders;
        
        private LocalQuestHolderData _localDialogData = new();
        private const string _SAVE_CODE = "QUEST_HOLDERS_DATA";
        
        public QuestHolderSaveUploader(QuestHolder[] questHolders)
        {
            _questHolders = questHolders;
        }
        
        public async UniTask UploadSaves()
        {
            if (TryGetSaves())
            {
                if (_localDialogData.QuestHolderDatas.Count != _questHolders.Length)
                    throw new ApplicationException("Ошибка локального сохранения");

                for (var i = 0; i < _questHolders.Length; i++)
                {
                    _questHolders[i].DynamicInit(_localDialogData.QuestHolderDatas[i]);
                }

                await UniTask.Delay(TimeSpan.FromMilliseconds(15));
            }
            else
            {
                throw new ApplicationException("Ошибка локального сохранения");
            }
        }

        public void Dispose()
        {
            _localDialogData.QuestHolderDatas = _questHolders.Select(dialogPoint => dialogPoint.QuestHolderData).ToList();

            var save = JsonUtility.ToJson(_localDialogData);
            PlayerPrefs.SetString(_SAVE_CODE, save);
        }

        private bool TryGetSaves()
        {
            var json = PlayerPrefs.GetString(_SAVE_CODE);
            var instance = JsonUtility.FromJson<LocalQuestHolderData>(json);

            if (instance == null) return false;

            _localDialogData = instance;

            return true;
        }
    }
}