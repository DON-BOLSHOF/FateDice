using System;
using BKA.BattleDirectory.ReadinessObserver;
using BKA.UI;
using UniRx;
using UnityEngine;
using Zenject;

namespace BKA.BattleDirectory.BattleSystems
{
    public class StateSystem : MonoBehaviour, IFinilizerLockedSystem
    {
        [Inject] private TurnSystem _turnSystem;
        [Inject] private ReadinessToNextTurnObservable _readinessToNextTurn;

        [SerializeField] private RerollWidget _rerollWidget;
        [SerializeField] private ReadyWidget _readyWidget;
        [SerializeField] private TurnWidget _turnWidget;
        [SerializeField] private UndoWidget _undoWidget;

        [SerializeField] private CharacterStateWidget[] _characterStateWidgets;

        private ReactiveProperty<bool> _isPartyTurn = new(false);
        
        private void Start()
        {
            _turnSystem.TurnState.Subscribe(OnTurnChanged).AddTo(this);

            _isPartyTurn.CombineLatest(_readinessToNextTurn.IsReadyEmergency, (b, b1) => b && b1).Where(value => value)
                .Subscribe(_ =>
                {
                    _readyWidget.gameObject.SetActive(false);
                    _turnWidget.gameObject.SetActive(true);
                    _undoWidget.gameObject.SetActive(true);
                    _rerollWidget.gameObject.SetActive(false);
                    
                    foreach (var characterStateWidget in _characterStateWidgets)
                    {
                        characterStateWidget.StateButton.enabled = false;
                    }
                }).AddTo(this);
            _isPartyTurn.CombineLatest(_readinessToNextTurn.IsReadyEmergency, (b, b1) => b && !b1).Where(value => value)
                .Subscribe(_ =>
                {
                    _readyWidget.gameObject.SetActive(true);
                    _rerollWidget.gameObject.SetActive(true);
                    _turnWidget.gameObject.SetActive(false);
                    _undoWidget.gameObject.SetActive(false);
                    
                    foreach (var characterStateWidget in _characterStateWidgets)
                    {
                        characterStateWidget.StateButton.enabled = true;
                    }
                }).AddTo(this);
        }

        private void OnTurnChanged(TurnState turnState)
        {
            switch (turnState)
            {
                case TurnState.PartyTurn:
                    _isPartyTurn.Value = true;
                    break;
                case TurnState.EnemyTurn:
                    _isPartyTurn.Value = false;
                    _readyWidget.gameObject.SetActive(false);
                    _rerollWidget.gameObject.SetActive(false);
                    _turnWidget.gameObject.SetActive(false);
                    _undoWidget.gameObject.SetActive(false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(turnState), turnState, null);
            }
        }

        public void Lock()
        {
            _readyWidget.enabled  = false;
            _rerollWidget.enabled  = false;
            _turnWidget.enabled  = false;
            _undoWidget.enabled  = false;
            
            foreach (var characterStateWidget in _characterStateWidgets)
            {
                characterStateWidget.enabled = false;
            }
        }
    }
}