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
        private Vector3 _basePosition;
        
        private const string _SAVE_CODE = "HERO_POSITION_DATA";

        public HeroPositionSaveUploader(HeroPositionComponent positionComponent)
        {
            _heroPositionComponent = positionComponent;
            _basePosition = positionComponent.transform.position;
        }

        public void UploadBaseSaves()
        {
            _heroPositionComponent.DynamicInit(_basePosition);
        }

        public async UniTask UploadLocalSaves()
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