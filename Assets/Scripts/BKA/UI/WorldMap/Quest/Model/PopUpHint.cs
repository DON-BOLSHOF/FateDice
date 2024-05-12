using System;
using System.Collections;
using BKA.Utils;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace BKA.UI.WorldMap
{
    public class PopUpHint : MonoBehaviour
    {
        [SerializeField] protected Graphic[] _graphics;
        [SerializeField] private int _popUpTimerTime;
        [SerializeField] private float _popUpIntervalTime = 0.07f;

        public IObservable<Unit> OnShown => _onShown;
        public IObservable<Unit> OnDisappeared => _onDisappeared;
        
        protected Coroutine _graphicRoutine;
        protected Timer _timer;
        
        private float _currentAlpha;
        
        public bool IsTimerRunning => _timer.IsRunning;
        public int RemainingSeconds => _timer.RemainingSeconds;

        private ReactiveCommand _onShown = new();
        private ReactiveCommand _onDisappeared = new();
            
        private void Awake()
        {
            _timer = new Timer(_popUpTimerTime);
            _timer.OnTimeExpired.Subscribe(_ => Exit()).AddTo(this);
            _timer.OnTimeExpired.Subscribe(_ => Exit()).AddTo(this);
        }

        public virtual void Show()
        {
            StartRoutine(Show(_graphics), ref _graphicRoutine);
            _timer.ReloadTimer();
        }

        public virtual void ForceExit()
        {
            _timer.StopTimer();
            Exit();
        }

        protected virtual void Exit()
        {
            StartRoutine(Disappear(_graphics), ref _graphicRoutine);
        }

        protected virtual IEnumerator Show(Graphic[] graphics)
        {
            for (; _currentAlpha <= 1.05f; _currentAlpha += 0.05f)
            {
                foreach (var graphic in graphics)
                {
                    var variableColor = graphic.color;
                    variableColor.a = _currentAlpha;
                    graphic.color = variableColor;
                }

                yield return new WaitForSeconds(_popUpIntervalTime);
            }

            _currentAlpha = 1;
            _onShown?.Execute();
        }

        protected virtual IEnumerator Disappear(Graphic[] graphics)
        {
            for (; _currentAlpha >= -0.05; _currentAlpha -= 0.05f)
            {
                foreach (var graphic in graphics)
                {
                    var variableColor = graphic.color;
                    variableColor.a = _currentAlpha;
                    graphic.color = variableColor;
                }

                yield return new WaitForSeconds(_popUpIntervalTime);
            }

            _currentAlpha = 0;
            _onDisappeared?.Execute();
        }

        protected void ChangeGraphicsAlphaTo(float value)
        {
            _currentAlpha = value;
            foreach (var graphic in _graphics)
            {
                var variableColor = graphic.color;
                variableColor.a = _currentAlpha;
                graphic.color = variableColor;
            }
        }
        
        public void ReloadTimer()
        {
            _timer?.ReloadTimer();
        }

        public void StopTimer()
        {
            _timer?.StopTimer();
        }

        protected void StartRoutine(IEnumerator coroutine, ref Coroutine routine)
        {
            if (routine != null)
                StopCoroutine(routine);

            routine = StartCoroutine(coroutine);
        }
        
#if UNITY_EDITOR

        [Button(ButtonSizes.Small)]
        private void GetGraphics()
        {
            _graphics = GetComponentsInChildren<Graphic>();
        }

        [Button(ButtonSizes.Small)]
        private void AlphaToZero()
        {
            ChangeGraphicsAlphaTo(0);
        }

        [Button(ButtonSizes.Small)]
        private void AlphaToOne()
        {
            ChangeGraphicsAlphaTo(1);
        }
#endif
    }
}