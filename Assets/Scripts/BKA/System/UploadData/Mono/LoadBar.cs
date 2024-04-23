using UniRx;
using UnityEngine;
using Zenject;

namespace BKA.System.UploadData.Mono
{
    public class LoadBar : MonoBehaviour
    {
        [Inject] private ISynchronizer _synchronizer;

        private void Start()
        {
            _synchronizer.IsSynchrolized.Where(value => value).Subscribe(_ => gameObject.SetActive(false)).AddTo(this);
        }
    }
}