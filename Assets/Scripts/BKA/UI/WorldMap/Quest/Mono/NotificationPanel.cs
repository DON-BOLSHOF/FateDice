using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BKA.Utils.CodeAnimation;
using ModestTree;
using UnityEngine;
using UniRx;

namespace BKA.UI.WorldMap
{
    public class NotificationPanel : MonoBehaviour, INotificationPanel
    {
        [Serializable]
        private class RaiseNotificationWidget
        {
            [SerializeField] private NotificationWidget _notificationWidget;

            public NotificationWidget NotificationWidget => _notificationWidget;
            public Coroutine RaiseRoutine;
        }

        [SerializeField] private RaiseNotificationWidget[] _notificationWidgets;

        [SerializeField] private Vector3[] _raisePositions;

        [SerializeField] private Vector3 _basePosition;

        private Queue<Notification> _notifications = new Queue<Notification>();

        private int _notificateIndex = 0;

        private Coroutine _notificationCoroutine;

        private void Start()
        {
            foreach (var widget in _notificationWidgets)
            {
                widget.NotificationWidget.OnDisappeared.Subscribe(_ =>
                {
                    SetWidgetBasePosition(widget.NotificationWidget);
                    ReloadRaisingWidgets();
                }).AddTo(this);
            }
        }

        public void SentNotification(Notification notification)
        {
            _notifications.Enqueue(notification);

            StartRoutine(StartNotificate(), ref _notificationCoroutine);
        }
        
        public void SentNotification(Notification[] notifications)
        {
            foreach (var notification in notifications)
            {
                _notifications.Enqueue(notification);
            }

            StartRoutine(StartNotificate(), ref _notificationCoroutine);
        }

        private IEnumerator StartNotificate()
        {
            yield return new WaitForSeconds(RemainingSeconds());

            while (!_notifications.IsEmpty())
            {
                yield return new WaitForSeconds(RemainingSeconds());

                var notification = _notifications.Dequeue();
                _notificationWidgets[_notificateIndex].NotificationWidget.SetData(notification);
                _notificationWidgets[_notificateIndex].NotificationWidget.Show();

                ReloadRaisingWidgets();

                _notificateIndex++;
                _notificateIndex %= 3;
                yield return new WaitForSeconds(2);
            }
        }

        private void ReloadRaisingWidgets() //Пиздец макаронина
        {
            var leftNotifications = _notificationWidgets.Where(t1 => t1.NotificationWidget.IsTimerRunning).ToArray();

            switch (leftNotifications.Length)
            {
                case 0:
                    return;
                case 1:
                    StartRoutine(RaiseAnimation.RaiseRectTransformObject(
                        leftNotifications[0].NotificationWidget.GetComponent<RectTransform>(),
                        _raisePositions[0]), ref leftNotifications[0].RaiseRoutine);
                    break;
                case 2:
                    var indexMin = leftNotifications[0].NotificationWidget.RemainingSeconds <
                                   leftNotifications[1].NotificationWidget.RemainingSeconds
                        ? 0
                        : 1;
                    StartRoutine(RaiseAnimation.RaiseRectTransformObject(
                        leftNotifications[indexMin].NotificationWidget.GetComponent<RectTransform>(),
                        _raisePositions[0]), ref leftNotifications[indexMin].RaiseRoutine);

                    var indexMax = indexMin == 0 ? 1 : 0;
                    StartRoutine(RaiseAnimation.RaiseRectTransformObject(
                        leftNotifications[indexMax].NotificationWidget.GetComponent<RectTransform>(),
                        _raisePositions[1]), ref leftNotifications[indexMax].RaiseRoutine);
                    break;
            }
        }

        private void SetWidgetBasePosition(NotificationWidget notificationWidget)
        {
            notificationWidget.GetComponent<RectTransform>().anchoredPosition = _basePosition;
        }

        private int RemainingSeconds()
        {
            var remainingFirst = _notificationWidgets[0].NotificationWidget.RemainingSeconds;
            var remainingSecond = _notificationWidgets[1].NotificationWidget.RemainingSeconds;
            var remainingThird = _notificationWidgets[2].NotificationWidget.RemainingSeconds;

            if (_notificationWidgets[0].NotificationWidget.IsTimerRunning &&
                _notificationWidgets[1].NotificationWidget.IsTimerRunning)
                return Math.Min(remainingFirst, remainingSecond);
            if (_notificationWidgets[1].NotificationWidget.IsTimerRunning &&
                _notificationWidgets[2].NotificationWidget.IsTimerRunning)
                return Math.Min(remainingSecond, remainingThird);
            if (_notificationWidgets[0].NotificationWidget.IsTimerRunning &&
                _notificationWidgets[2].NotificationWidget.IsTimerRunning)
                return Math.Min(remainingFirst, remainingThird);

            return 0;
        }

        private void StartRoutine(IEnumerator coroutine, ref Coroutine routine)
        {
            if (routine != null)
                StopCoroutine(routine);

            routine = StartCoroutine(coroutine);
        }
    }
}
