using UniRx;

namespace BKA.System
{
    public interface ISyncSystem
    {
        public IReadOnlyReactiveProperty<bool> IsSynchrolized { get; }

        public void Synchronize(BattleSynchronizer synchronizer);
    }
}