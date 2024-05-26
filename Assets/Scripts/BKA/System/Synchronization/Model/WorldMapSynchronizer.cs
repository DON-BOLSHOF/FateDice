using BKA.System.Synchronization.Interfaces;
using UniRx;

namespace BKA.System.UploadData
{
    public class WorldMapSynchronizer : IWorldMapSynchronizer
    {
        public ReadOnlyReactiveProperty<bool> IsSynchrolized { get; }

        public WorldMapSynchronizer(NavigationSynchronizer navigationSynchronizer, UploadHandlerSynchronizer uploadHandlerSynchronizer)
        {
            IsSynchrolized = navigationSynchronizer.IsSynchrolized.CombineLatest(uploadHandlerSynchronizer.IsSynchrolized,
                (b, b1) => b&& b1).ToReadOnlyReactiveProperty();
        }
    }
}