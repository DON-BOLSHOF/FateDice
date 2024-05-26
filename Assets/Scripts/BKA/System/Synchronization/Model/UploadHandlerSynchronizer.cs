using System;
using UniRx;

namespace BKA.System.UploadData
{
    public class UploadHandlerSynchronizer : ISynchronizer, IDisposable
    {
        public ReadOnlyReactiveProperty<bool> IsSynchrolized { get;}

        private ReactiveProperty<bool> _isSynchrolized = new();
        
        private CompositeDisposable _synchonizerDisposable = new();
        
        public UploadHandlerSynchronizer(UploadHandler uploadHandler)
        {
            IsSynchrolized = _isSynchrolized.ToReadOnlyReactiveProperty();
            
            uploadHandler.IsReadyAbsolutely.Where(value => value).Subscribe(_ => _isSynchrolized.Value = true).AddTo(_synchonizerDisposable);
        }

        public void Dispose()
        {
            _isSynchrolized?.Dispose();
            _synchonizerDisposable?.Dispose();
            IsSynchrolized?.Dispose();
        }
    }
}