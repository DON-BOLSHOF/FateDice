using System;
using BKA.System;
using UniRx;
using Zenject;

namespace BKA.BattleDirectory.ReadinessObserver
{
    public class ReadinessToBattleObservable: IReadinessObservable, IInitializable, IDisposable
    {
        public ReadOnlyReactiveProperty<bool> IsReadyAbsolutely { get; private set; }

        [Inject] private Synchronizer _synchronizer;
        
        public void Initialize()
        {
            IsReadyAbsolutely = _synchronizer.IsSynchrolized.ToReadOnlyReactiveProperty();//Потом расширится думаю
        }

        public void Dispose()
        {
            IsReadyAbsolutely?.Dispose();
        }
    }
}