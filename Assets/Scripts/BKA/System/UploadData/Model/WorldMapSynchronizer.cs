using System;
using BKA.System;
using UniRx;

namespace BKA.TestUploadData
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