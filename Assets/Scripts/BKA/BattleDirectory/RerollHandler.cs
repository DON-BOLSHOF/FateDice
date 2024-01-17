using System;
using System.Collections.Generic;
using BKA.Dices;
using BKA.System.Exceptions;
using BKA.UI;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace BKA.BattleDirectory
{
    public class RerollHandler : MonoBehaviour
    {
        [SerializeField] private ShakeSystem _shakeSystem;

        [SerializeField] private int _totalRerolls = 2;
        private ReactiveProperty<int> _remainRerolls = new();

        [SerializeField] private RerollWidget _rerollWidget;

        private List<DiceObject> _activeDices;

        private ReactiveProperty<bool> _isDicesReadyToReroll = new(true);
        public IReadOnlyReactiveProperty<bool> IsDicesReady => _isDicesReadyToReroll;

        private void Start()
        {
            _isDicesReadyToReroll.Where(value => !value).Subscribe(_ =>
                    WaitReadiness(_activeDices).Forget()).AddTo(this);

            _rerollWidget.DynamicInit(_remainRerolls, _totalRerolls);

            _rerollWidget.OnRerolled.Subscribe(_ => Reroll()).AddTo(this);
        }

        private void Reroll()
        {
            if (_remainRerolls.Value > 0 && _isDicesReadyToReroll.Value)
            {
                _shakeSystem.ShakeObjects(_activeDices);

                _isDicesReadyToReroll.Value = false;

                _remainRerolls.Value--;
            }
        }

        private async UniTaskVoid WaitReadiness(List<DiceObject> diceObjects)
        {
            await UniTask.Delay(TimeSpan.FromMilliseconds(25));

            foreach (var diceObject in diceObjects)
            {
                await UniTask.WaitUntil(() => diceObject.Rigidbody.velocity.magnitude <= 0.025f);
            }

            _isDicesReadyToReroll.Value = true;
        }

        public void UpdateDices(List<DiceObject> diceObjects)
        {
            _activeDices = diceObjects;

            _remainRerolls.Value = _totalRerolls;
        }

        public void ForceReroll()
        {
            if (!_isDicesReadyToReroll.Value)
                throw new RerollException();
                
            _shakeSystem.ShakeObjects(_activeDices);
        }
    }
}