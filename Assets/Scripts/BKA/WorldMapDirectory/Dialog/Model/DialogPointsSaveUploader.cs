using System;
using System.Collections.Generic;
using System.Linq;
using BKA.System.UploadData;
using BKA.WorldMapDirectory.Dialog.Model;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BKA.UI.WorldMap.Dialog
{
    public class LocalDialogPointsData
    {
        public List<DialogPointData> DialogPointDatas;
    }
    
    public class DialogPointsSaveUploader  : ISaveUploader, IDisposable
    {
        private DialogPoint[] _dialogPoints;
        
        private LocalDialogPointsData _localDialogData = new();
        private const string _SAVE_CODE = "DIALOG_POINTS_DATA";

        public DialogPointsSaveUploader(DialogPoint[] dialogPoints)
        {
            _dialogPoints = dialogPoints;
        }
        
        public async UniTask UploadSaves()
        {
            if (TryGetSaves())
            {
                if (_localDialogData.DialogPointDatas.Count != _dialogPoints.Length)
                    throw new ApplicationException("Ошибка локального сохранения");

                for (var i = 0; i < _dialogPoints.Length; i++)
                {
                    _dialogPoints[i].DynamicInit(_localDialogData.DialogPointDatas[i]);
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
            _localDialogData.DialogPointDatas = _dialogPoints.Select(dialogPoint => dialogPoint.DialogPointData).ToList();

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