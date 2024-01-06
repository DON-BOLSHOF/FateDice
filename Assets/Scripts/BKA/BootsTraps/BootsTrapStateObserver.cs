using UniRx;

namespace BKA.BootsTraps
{
    public sealed class BootsTrapStateObserver
    {
        private ReactiveProperty<BootsTrapState> _bootTrapState { get; } = new();

        public IReactiveProperty<BootsTrapState> BootsTrapState => _bootTrapState;

        public void Visit(BootsTrap visitor)
        {
            _bootTrapState.Value = BootsTraps.BootsTrapState.Loaded;
        }
    }

    public enum BootsTrapState
    {
        NonLoad,
        Loaded
    }
}