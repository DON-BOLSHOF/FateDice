using BKA.Buffs;
using Cysharp.Threading.Tasks;

namespace BKA.System
{
    public class ArtefactProvider : LocalAssetLoader<Artefact>
    {
        public UniTask<Artefact> Load(string assetId)
        {
            return LoadInternal(assetId);
        }

        public void Unload()
        {
            Release();
        }
    }
}