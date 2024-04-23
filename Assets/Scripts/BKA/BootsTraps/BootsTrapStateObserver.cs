using UniRx;

namespace BKA.BootsTraps
{
    public sealed class BootsTrapStateObserver
    {
        private ReactiveProperty<BootsTrapState> _bootTrapState { get; } = new();

        public IReadOnlyReactiveProperty<BootsTrapState> BootsTrapState => _bootTrapState;

        public void Visit(BootsTrap visitor)
        {
            _bootTrapState.Value = BootsTraps.BootsTrapState.Loaded;
        }

        public void TestVisit()
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