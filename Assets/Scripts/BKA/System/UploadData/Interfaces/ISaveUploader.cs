using Cysharp.Threading.Tasks;

namespace BKA.TestUploadData
{
    public interface ISaveUploader
    {
        UniTask UploadSaves();
    }
}