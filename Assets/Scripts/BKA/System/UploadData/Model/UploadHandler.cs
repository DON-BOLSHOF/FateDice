using System;
using System.Collections.Generic;
using BKA.BattleDirectory.ReadinessObserver;
using Cysharp.Threading.Tasks;
using UniRx;
using Zenject;

namespace BKA.TestUploadData
{
    public class UploadHandler : IReadinessObservable
    {
        public ReadOnlyReactiveProperty<bool> IsReadyAbsolutely => _isReadyAbsolutely.ToReadOnlyReactiveProperty();

        private readonly ReactiveProperty<bool> _isReadyAbsolutely = new(false);
        
        public UploadHandler(IEnumerable<ISaveUploader> saveUploaders, [InjectOptional] SystemStage systemStage)
        {
            UploadData(saveUploaders, systemStage).Forget();
        }

        private async UniTask UploadData(IEnumerable<ISaveUploader> saveUploaders, SystemStage systemStage)
        {
            switch (systemStage)
            {
                case SystemStage.NewGame:
                    break;
                case SystemStage.LocalChanges:
                    foreach (var saveUploader in saveUploaders)
                    {
                        await saveUploader.UploadSaves();
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(systemStage), systemStage, null);
            }

            _isReadyAbsolutely.Value = true;
        }
    }
}