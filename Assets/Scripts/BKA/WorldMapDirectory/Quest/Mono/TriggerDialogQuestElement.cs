using System;
using BKA.UI.WorldMap.Dialog;
using UniRx;
using UnityEngine;

namespace BKA.WorldMapDirectory.Quest
{
    public class TriggerDialogQuestElement : QuestElement
    {
        [SerializeField] private Transform _hintObject;
        [SerializeField] private string _questElementHint;
        [SerializeField] private TriggerDialogPoint _triggerDialogPoint;

        public IObservable<(CharacterPhraseProvider[],Action)> OnActivateDialog => _onActivateDialog;
        public override string QuestElementHint => _questElementHint;

        private readonly ReactiveCommand<(CharacterPhraseProvider[],Action)> _onActivateDialog = new();

        private void Start()
        {
            _triggerDialogPoint.gameObject.SetActive(false);
            _triggerDialogPoint.OnActivatedDialog.Subscribe(_ =>
            {
                _onActivateDialog?.Execute((_triggerDialogPoint.CharacterPhraseProviders,OnDialogEnded));
            }).AddTo(this);
        }

        public override void Activate()
        {
            _triggerDialogPoint.gameObject.SetActive(true);
            _hintObject.gameObject.SetActive(true);
        }

        private void OnDialogEnded()
        {
            _onElementCompleted?.Execute();
            _hintObject.gameObject.SetActive(false);
        }
    }
}