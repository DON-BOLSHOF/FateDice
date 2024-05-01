using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BKA.UI.WorldMap
{
    public class SweepAttribute : MonoBehaviour
    {
        [SerializeField] private Image _action;
        [SerializeField] private Image _stateView;
        [SerializeField] private TextMeshProUGUI _actionValue;

        private CancellationTokenSource _sweepSource = new();

        private Sprite _attributeView;
        private int _modificatorValue;

        public void UpdateData(Sprite view, int modificatorValue)
        {
            _attributeView = view;
            _modificatorValue = modificatorValue;

            UpdateLocalData();
        }

        public void MakeHint(Sprite hintSprite)
        {
            DeactivateHint();
            _sweepSource = new();
            
            _stateView.gameObject.SetActive(false);
            _action.sprite = hintSprite;

            ActivateHint(_sweepSource.Token).Forget();
        }

        public void DeactivateHint()
        {
            _sweepSource?.Cancel();

            _action.color = Color.white;

            UpdateLocalData();
        }

        private void UpdateLocalData()
        {
            _action.sprite = _attributeView;
            if (_modificatorValue > 0)
            {
                _actionValue.text = _modificatorValue.ToString();
                _stateView.gameObject.SetActive(true);
            }
            else
            {
                _stateView.gameObject.SetActive(false);
            }
        }

        private async UniTaskVoid ActivateHint(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await _action.DOColor(Color.blue, 1).ToUniTask(cancellationToken: token);
                await _action.DOColor(Color.cyan, 1).ToUniTask(cancellationToken: token);
            }
        }
        
        private void OnDestroy()
        {
            _sweepSource?.Cancel();
        }
    }
}