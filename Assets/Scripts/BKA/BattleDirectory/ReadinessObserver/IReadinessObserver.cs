using UniRx;

namespace BKA.BattleDirectory.ReadinessObserver
{
    public interface IReadinessObserver
    {
        ReadOnlyReactiveProperty<bool> IsReady { get; }
    }
}