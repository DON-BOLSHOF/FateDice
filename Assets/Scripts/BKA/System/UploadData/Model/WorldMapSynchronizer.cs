using UniRx;

namespace BKA.System.UploadData
{
    public class WorldMapSynchronizer : ISynchronizer
    {
        public ReadOnlyReactiveProperty<bool> IsSynchrolized { get; }

        public WorldMapSynchronizer(UploadHandler uploadHandler)
        {
            IsSynchrolized = uploadHandler.IsReadyAbsolutely.ToReadOnlyReactiveProperty();
        }
    }
}