using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace BKA.System
{
    public class LocalAssetLoader<T>
    {
        private T _cachedObject;
        
        protected async UniTask<T> LoadInternal(string assetId)
        {
            var handle = Addressables.LoadAssetAsync<T>(assetId); 
            _cachedObject = await handle;
            
            return _cachedObject;
        }

        protected void Release()
        {
            if(_cachedObject == null)
                return;

            Addressables.Release(_cachedObject);
            _cachedObject = default;
        }
    }
}