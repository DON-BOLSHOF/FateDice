using System;
using UniRx;
using Zenject;

namespace BKA.System
{
    public class ReadinessToBattleObserver: IInitializable, IDisposable
    {
        public ReadOnlyReactiveProperty<bool> IsReadyToBattle;

        [Inject] private Synchronizer _synchronizer;
        
        public void Initialize()
        {
            IsReadyToBattle = _synchronizer.IsSynchrolized.ToReadOnlyReactiveProperty();//Потом расширится думаю
        }

        public void Dispose()
        {
            IsReadyToBattle?.Dispose();
        }
    }
}