using UniRx;

namespace BKA.System
{
    public interface ISynchronizer
    {
        ReadOnlyReactiveProperty<bool> IsSynchrolized { get; }
    }
}