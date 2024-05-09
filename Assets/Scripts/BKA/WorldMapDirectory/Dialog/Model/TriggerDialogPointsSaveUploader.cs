using System;
using System.Collections.Generic;
using System.Linq;
using BKA.System.UploadData;
using BKA.WorldMapDirectory.Systems;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BKA.UI.WorldMap.Dialog
{
    public class LocalDialogPointsData
    {
        public List<DialogPointData> DialogPointDatas;
    }
    
    public class TriggerDialogPointsSaveUploader  : ISaveUploader, IDisposable
    {
        private TriggerDialogPoint[] _triggerDialogPoints;
        
        private LocalDialogPointsData _localDialogData = new();
        private const string _SAVE_CODE = "TRIGGER_DIALOG_POINTS_DATA";

        public TriggerDialogPointsSaveUploader(TriggerDialogPoint[] triggerDialogPoints)
        {
            _triggerDialogPoints = triggerDialogPoints;
        }
        
        public async UniTask UploadSaves()
        {
            if (TryGetSaves())
            {
                if (_localDialogData.DialogPointDatas.Count != _triggerDialogPoints.Length)
                    throw new ApplicationException("Ошибка локального сохранения");

                for (var i = 0; i < _triggerDialogPoints.Length; i++)
                {
                    _triggerDialogPoints[i].DynamicInit(_localDialogData.DialogPointDatas[i]);
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
            _localDialogData.DialogPointDatas = _triggerDialogPoints.Select(dialogPoint => dialogPoint.DialogPointData).ToList();

            var save = JsonUtility.ToJson(_localDialogData);
            PlayerPrefs.SetString(_SAVE_CODE, save);
        }

        private bool TryGetSaves()
        {
            var json = PlayerPrefs.GetString(_SAVE_CODE);
            var instance = JsonUtility.FromJson<LocalDialogPointsData>(json);

            if (instance == null) return false;

            _localDialogData = instance;

            return true;
        }
    }
}