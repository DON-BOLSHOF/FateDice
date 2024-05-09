using UnityEngine;
using UnityEngine.UI;

namespace BKA.UI
{
    public class ActorWidget : MonoBehaviour
    {
        [SerializeField] private Image _view;

        public void SetData(Sprite view)
        {
            _view.sprite = view;
            gameObject.SetActive(true);
        }

        public void ClearData()
        {
            _view.sprite = null;
            gameObject.SetActive(false);
        }
    }
}