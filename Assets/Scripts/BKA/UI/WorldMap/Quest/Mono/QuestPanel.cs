using System;
using BKA.UI.WorldMap.Quest.Interfaces;
using BKA.WorldMapDirectory.Quest;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace BKA.UI.WorldMap.Quest.Mono
{
    public class QuestPanel : MonoBehaviour, IQuestPanel
    {
        [SerializeField] private Transform _view;

        [SerializeField] private TextMeshProUGUI _questName;
        [SerializeField] private TextMeshProUGUI _description;
        
        [SerializeField] private Button _activateButton;
        [SerializeField] private Button _refusalButton;

        [SerializeField] private TextMeshProUGUI _activateButtonText;

        public IObservable<bool> OnActivateQuest => _onActivateQuest;

        private ReactiveCommand<bool> _onActivateQuest = new();

        private void Start()
        {
            _activateButton.OnClickAsObservable().Subscribe(_ =>
            {
                _onActivateQuest?.Execute(true);
                DeactivatePanel();
            }).AddTo(this);
            _refusalButton.OnClickAsObservable().Subscribe(_ =>
            {
                _onActivateQuest?.Execute(false);
                DeactivatePanel();
            }).AddTo(this);
        }

        public void ActivatePanel(QuestInterlude interlude)
        {
            _questName.text = interlude.QuestName;
            _description.text = interlude.Description;

            _activateButtonText.text = interlude.ActivationButtonDescription;
            
            _refusalButton.gameObject.SetActive(interlude.IsAdditionalQuest);

            _view.gameObject.SetActive(true);
        }

        private void DeactivatePanel()
        {
            _view.gameObject.SetActive(false);
        }
    }
}