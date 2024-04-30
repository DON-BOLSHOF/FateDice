using System;
using BKA.System.UploadData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BKA.WorldMapDirectory.Systems
{
    public class HeroPositionSaveUploader : ISaveUploader, IDisposable
    {
        private HeroPositionComponent _heroPositionComponent;

        private Vector3 _localPosition;
        
        private const string _SAVE_CODE = "HERO_POSITION_DATA";

        public HeroPositionSaveUploader(HeroPositionComponent positionComponent)
        {
            _heroPositionComponent = positionComponent;
        }

        public async UniTask UploadSaves()
        {
            if (TryGetSaves())
            {
                _heroPositionComponent.DynamicInit(_localPosition);

                await UniTask.Delay(TimeSpan.FromMilliseconds(15));
            }
        }

        public void Dispose()
        {
            var position = _heroPositionComponent.transform.position;
            
            var save = JsonUtility.ToJson(position);
            PlayerPrefs.SetString(_SAVE_CODE, save);
        }

        private bool TryGetSaves()
        {
            var json = PlayerPrefs.GetString(_SAVE_CODE);
            var position = JsonUtility.FromJson<Vector3>(json);

            _localPosition = position;

            return true;
        }
    }
}