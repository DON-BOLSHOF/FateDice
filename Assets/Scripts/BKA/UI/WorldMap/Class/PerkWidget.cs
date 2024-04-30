using System;
using System.Threading;
using BKA.Buffs;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace BKA.UI
{
    [RequireComponent(typeof(Button))]
    public class PerkWidget : MonoBehaviour
    {
        [SerializeField] private Button _selectButton;
        [SerializeField] private Image _perkView;
        [SerializeField] private Image _perkSelectedView;

        public IObservable<Specialization> OnSelectedPerk => _onSelectedPerk;

        private Specialization _specialization;
        private ReactiveCommand<Specialization> _onSelectedPerk = new();

        private CancellationTokenSource _cancellationTokenSource = new();

        private void Start()
        {
            _selectButton.OnClickAsObservable().Subscribe(_ => _onSelectedPerk?.Execute(_specialization)).AddTo(this);
        }

        public void UpdateData(Specialization specialization)
        {
            _perkView.sprite = specialization.Definition.View;
            _specialization = specialization;
        }

        public void SelectView()
        {
            _perkSelectedView.color = Color.cyan;
        }

        public void UnSelectView()
        {
            _perkSelectedView.color = Color.black;
        }

        public void LockInput(bool value)
        {
            _selectButton.enabled = !value;
        }

        public void ClearData()
        {
            _perkView.sprite = null;
            _specialization = null;
        }

        public void ActivateHint()
        {
            _perkView.color = Color.grey;
            _perkSelectedView.color = Color.black;

            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new();
            Hinting(_cancellationTokenSource.Token).Forget();
        }

        private async UniTaskVoid Hinting(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await transform.DOScale(new Vector3(1.25f, 1.25f, 1.25f), 1).ToUniTask(cancellationToken: token);
                await transform.DOScale(new Vector3(1f, 1f, 1f), 1).ToUniTask(cancellationToken: token);
            }
        }

        public void DeactivateHint()
        {
            _perkView.color = Color.white;
            _perkSelectedView.color = Color.black;

            _cancellationTokenSource?.Cancel();
            transform.localScale = Vector3.one;
        }

        private void OnDestroy()
        {
            _cancellationTokenSource?.Cancel();
        }
    }
}