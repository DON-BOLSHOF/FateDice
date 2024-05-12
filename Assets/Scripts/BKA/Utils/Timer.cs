using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;

namespace BKA.Utils
{
    public class Timer
    {
        private enum TimerStatus
        {
            Deactive,
            Active
        }

        public bool IsRunning => _timerStatusState == TimerStatus.Active;

        public int RemainingSeconds => IsRunning ? _totalTime - _currentTime : 0;

        public IObservable<Unit> OnTimeExpired => _onTimeExpired;

        private int _totalTime;

        private TimerStatus _timerStatusState = TimerStatus.Deactive;
        private CancellationTokenSource _cancellationTokenSource = new();
        private ReactiveCommand _onTimeExpired = new();
        private int _currentTime;

        public Timer(int time)
        {
            _totalTime = time;
        }

        public async void ReloadTimer()
        {
            StopTimer();
            _cancellationTokenSource = new();

            _timerStatusState = TimerStatus.Active;
            _currentTime = 0;

            while (_currentTime < _totalTime)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(1));
                if (_cancellationTokenSource.Token.IsCancellationRequested) return;
                _currentTime++;
            }

            _onTimeExpired?.Execute();
            _timerStatusState = TimerStatus.Deactive;
        }

        public void StopTimer()
        {
            if (_cancellationTokenSource == null || _cancellationTokenSource.IsCancellationRequested) return;
            _timerStatusState = TimerStatus.Deactive;
            _cancellationTokenSource?.Cancel();
        }
    }
}