using System;
using UniRx;

namespace BKA.System.UploadData
{
    public class NavigationSynchronizer : ISynchronizer, IDisposable
    {
        public ReadOnlyReactiveProperty<bool> IsSynchrolized { get;}

        private ReactiveProperty<bool> _isSynchrolized = new();
        
        private CompositeDisposable _synchonizerDisposable = new();
        
        public NavigationSynchronizer(UploadHandlerSynchronizer uploadHandler)
        {
            IsSynchrolized = _isSynchrolized.ToReadOnlyReactiveProperty();
            
            uploadHandler.IsSynchrolized.Where(value => value).Subscribe(_ => _isSynchrolized.Value = true).AddTo(_synchonizerDisposable);
        }

        public void Dispose()
        {
            _isSynchrolized?.Dispose();
            _synchonizerDisposable?.Dispose();
            IsSynchrolized?.Dispose();
        }
    }
}