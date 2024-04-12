using BKA.UI;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BKA.BattleDirectory.BattleSystems
{
    public class LoseHandler : MonoBehaviour
    {
        [SerializeField] private LosePanel _losePanel;

        private void Start()
        {
            _losePanel.gameObject.SetActive(false);
            _losePanel.OnLoseClicked.Subscribe(_ => RestartGame()).AddTo(this);
        }

        public void ActivateLosePanel()
        {
            _losePanel.gameObject.SetActive(true);
        }

        private void RestartGame()
        {
            SceneManager.LoadSceneAsync(0);
        }
    }
}