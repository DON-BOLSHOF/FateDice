using TMPro;
using UnityEngine;

namespace BKA.UI.WorldMap
{
    public class NotificationWidget : PopUpHint
    {
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private TextMeshProUGUI _requestText;

        public void SetData(Notification notification)
        {
            _descriptionText.text = notification.Description;
            _requestText.text = notification.RequestDescription;
        }
    }
}