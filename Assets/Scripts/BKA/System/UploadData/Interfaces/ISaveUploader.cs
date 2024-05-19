using Cysharp.Threading.Tasks;

namespace BKA.System.UploadData
{
    public interface ISaveUploader
    {
        UniTask UploadLocalSaves();
        void UploadBaseSaves();
    }
}