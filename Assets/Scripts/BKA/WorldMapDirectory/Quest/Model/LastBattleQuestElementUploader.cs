using System;
using System.Collections.Generic;
using BKA.System.UploadData;
using BKA.WorldMapDirectory.Quest;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace BKA.TestUploadData.Installers
{
    [Serializable]
    public class BattleQuestElementLocalData
    {
        public int HashCode;
    }

    public class LastBattleQuestElementUploader : ISaveUploader, IDisposable
    {
        private IEnumerable<BattleQuestElement> _battleQuestElements;

        private BattleQuestElementLocalData _localData = new();
        private const string _SAVE_CODE = "LAST_BATTLE_ELEMENT_DATA";

        private readonly CompositeDisposable _compositeDisposable = new();

        public LastBattleQuestElementUploader(IEnumerable<BattleQuestElement> elements)
        {
            _battleQuestElements = elements;

            foreach (var battleQuestElement in _battleQuestElements)
            {
                battleQuestElement.OnBattleActivated
                    .Subscribe(_ => _localData.HashCode = battleQuestElement.GetHashCode()).AddTo(_compositeDisposable);
            }
        }

        public async UniTask UploadSaves()
        {
            if (TryGetSaves())
            {
                foreach (var battleQuestElement in _battleQuestElements)
                {
                    if (!battleQuestElement.GetHashCode().Equals(_localData.HashCode)) continue;

                    battleQuestElement.CastEndBattle();
                    return;
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
            var save = JsonUtility.ToJson(_localData);
            PlayerPrefs.SetString(_SAVE_CODE, save);
            
            _compositeDisposable?.Dispose();
        }

        private bool TryGetSaves()
        {
            var json = PlayerPrefs.GetString(_SAVE_CODE);
            var instance = JsonUtility.FromJson<BattleQuestElementLocalData>(json);

            if (instance == null) return false;

            _localData = instance;

            return true;
        }
    }
}