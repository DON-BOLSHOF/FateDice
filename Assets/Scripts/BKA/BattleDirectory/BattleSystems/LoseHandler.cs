using BKA.System;
using BKA.UI;
using UniRx;
using UnityEngine;
using Zenject;

namespace BKA.BattleDirectory.BattleSystems
{
    public class LoseHandler : MonoBehaviour
    {
        [SerializeField] private LosePanel _losePanel;

        [Inject] private LevelManager _levelManager;

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
            _levelManager.LoadLevel("MainMenu");
        }
    }
}