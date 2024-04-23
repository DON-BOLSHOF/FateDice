using System;
using System.Collections.Generic;
using System.Linq;
using BKA.TestUploadData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BKA.WorldMapDirectory.Systems
{
    public class LocalBattlePointsData
    {
        public List<BattlePointData> BattlePointDatas;
    }
    
    public class BattlePointsSaveUploader : ISaveUploader, IDisposable
    {
        private BattlePoint[] _battlePoints;
        
        private LocalBattlePointsData _localBattleData = new();
        private const string _SAVE_CODE = "BATTLE_POINTS_DATA";
        
        public BattlePointsSaveUploader(IEnumerable<BattlePoint> battlePoints)
        {
            _battlePoints = battlePoints.ToArray();
        }
        
        public async UniTask UploadSaves()
        {
            if (TryGetSaves())
            {
                if (_localBattleData.BattlePointDatas.Count != _battlePoints.Length)
                    throw new ApplicationException("Ошибка локального сохранения");

                for (var i = 0; i < _battlePoints.Length; i++)
                {
                    _battlePoints[i].DynamicInit(_localBattleData.BattlePointDatas[i]);
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
            _localBattleData.BattlePointDatas = _battlePoints.Select(square => square.BattlePointData).ToList();

            var save = JsonUtility.ToJson(_localBattleData);
            PlayerPrefs.SetString(_SAVE_CODE, save);
        }

        private bool TryGetSaves()
        {
            var json = PlayerPrefs.GetString(_SAVE_CODE);
            var instance = JsonUtility.FromJson<LocalBattlePointsData>(json);

            if (instance == null) return false;

            _localBattleData = instance;

            return true;
        }
    }
}