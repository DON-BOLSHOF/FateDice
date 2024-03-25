using UniRx;

namespace BKA.BattleDirectory.ReadinessObserver
{
    public interface IReadinessObservable
    {
        ReadOnlyReactiveProperty<bool> IsReady { get; }
    }
}