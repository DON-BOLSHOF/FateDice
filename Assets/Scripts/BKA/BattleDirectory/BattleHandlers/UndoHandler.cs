using System;
using BKA.BattleDirectory.BattleSystems;
using BKA.UI;
using UniRx;
using UnityEngine;
using Zenject;

namespace BKA.BattleDirectory.BattleHandlers
{
    public class UndoHandler : MonoBehaviour
    {
        [SerializeField] private UndoWidget _readyWidget;

        [Inject] private TurnSystem _turnSystem;

        public ReactiveCommand OnUndo = new();
        
        private void Start()
        {
            _readyWidget.OnUndo.Subscribe(_ => OnReadyClicked()).AddTo(this);
        }

        private void OnReadyClicked()
        {
            switch (_turnSystem.TurnState.Value)
            {
                case TurnState.PartyTurn:
                    OnUndo?.Execute();
                    break;
                case TurnState.EnemyTurn:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}