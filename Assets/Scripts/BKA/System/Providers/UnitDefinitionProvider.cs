using BKA.Units;
using Cysharp.Threading.Tasks;

namespace BKA.System
{
    public class UnitDefinitionProvider : LocalAssetLoader<UnitDefinition>
    {
        public UniTask<UnitDefinition> Load(string assetId)
        {
            return LoadInternal(assetId);
        }

        public void Unload()
        {
            Release();
        }
    }
}