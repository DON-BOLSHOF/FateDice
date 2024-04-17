using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace BKA.UI.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _exitButton;

        [SerializeField] private ChoosePanel _choosePanel;

        private void Start()
        {
            _choosePanel.gameObject.SetActive(false);
            
            _startButton.OnClickAsObservable().Subscribe(_ => OnStartClicked()).AddTo(this);
            _exitButton.OnClickAsObservable().Subscribe(_ => OnExitClicked()).AddTo(this);
        }

        private void OnStartClicked()
        {
            _choosePanel.gameObject.SetActive(true);
        }
        
        private void OnExitClicked()
        {
            Application.Quit();
        }
    }
}