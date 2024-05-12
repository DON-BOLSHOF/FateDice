using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Unit = BKA.Units.Unit;

namespace BKA.UI
{
    public class UpdateXPPanel : MonoBehaviour, IUpdateXPPanel, IPointerClickHandler
    {
        [SerializeField] private Transform _view;

        [SerializeField] private Button _closeButton;

        [SerializeField] private UnitXPWidget[] _unitXpWidgets;

        public IObservable<UniRx.Unit> OnCompleted => _onCompleted;

        private readonly ReactiveCommand _onCompleted = new();
        private CancellationTokenSource _panelSource = new();

        private void Start()
        {
            _closeButton.OnClickAsObservable().Subscribe(_ =>
            {
                _view.gameObject.SetActive(false);
                _onCompleted?.Execute();
                _panelSource?.Cancel();
            }).AddTo(this);
        }

        public void ActivatePanel(Unit[] units, float[] xpFrom, float[] xpTo)
        {
            if (units.Length > _unitXpWidgets.Length)
                throw new ApplicationException("Слишком много объектов");

            var index = 0;

            for (; index < units.Length; index++)
            {
                _unitXpWidgets[index].DynamicInit(units[index]);
                _unitXpWidgets[index].gameObject.SetActive(true);
            }

            for (; index < _unitXpWidgets.Length; index++)
            {
                _unitXpWidgets[index].gameObject.SetActive(false);
            }

            _view.gameObject.SetActive(true);
            
            ActivateAnimation(units.Length, xpFrom, xpTo).Forget();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _panelSource?.Cancel();
        }

        private async UniTaskVoid ActivateAnimation(int length, float[] xpFrom, float[] xpTo)
        {
            _panelSource = new();

            var tasks = new List<UniTask>();

            for (var i = 0; i < length; i++)
            {
                tasks.Add(_unitXpWidgets[i].UpdateXP(xpFrom[i], xpTo[i], _panelSource.Token));
            }

            await UniTask.WhenAll(tasks);
        }
    }
}