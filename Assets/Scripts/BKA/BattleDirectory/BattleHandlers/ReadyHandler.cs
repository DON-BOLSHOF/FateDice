using System;
using BKA.BattleDirectory.BattleSystems;
using BKA.UI;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;

namespace BKA.BattleDirectory.BattleHandlers
{
    public class ReadyHandler : MonoBehaviour
    {
        [SerializeField] private ReadyWidget _readyWidget;

        [Inject] private TurnSystem _turnSystem;

        public ReactiveCommand OnReady = new();
        
        private void Start()
        {
            _readyWidget.OnReady.Subscribe(_ => OnReadyClicked()).AddTo(this);
        }

        private void OnReadyClicked()
        {
            switch (_turnSystem.TurnState.Value)
            {
                case TurnState.PartyTurn:
                    OnReady?.Execute();
                    break;
                case TurnState.EnemyTurn:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}