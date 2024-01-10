using System;
using System.Collections.Generic;
using BKA.BattleDirectory;
using BKA.Dices;
using UniRx;
using UnityEngine;
using Zenject;

namespace BKA
{
    public class DiceHandler : MonoBehaviour
    {
        [SerializeField] private ShakeSystem _shakeSystem;

        private List<DiceObject> _partyDices = new();

        private List<DiceObject> _enemyDices = new();

        [Inject] private TurnSystem _turnSystem;

        public void DynamicInit(List<DiceObject> partyDices, List<DiceObject> enemyDices)
        {
            _partyDices = partyDices;
            _enemyDices = enemyDices;

            _turnSystem.TurnState.Subscribe(ChangeDices).AddTo(this);
        }

        private void ChangeDices(TurnState turnState)
        {
            switch (turnState)
            {
                case TurnState.PartyTurn:
                    foreach (var diceObject in _enemyDices)
                    {
                        diceObject.gameObject.SetActive(false);
                    }

                    foreach (var diceObject in _partyDices)
                    {
                        diceObject.gameObject.SetActive(true);
                    }
                    break;
                case TurnState.EnemyTurn:
                    foreach (var diceObject in _enemyDices)
                    {
                        diceObject.gameObject.SetActive(true);
                    }

                    foreach (var diceObject in _partyDices)
                    {
                        diceObject.gameObject.SetActive(false);
                    }
                    break;
            }
        }

        public void Shake()
        {
            switch (_turnSystem.TurnState.Value)
            {
                case TurnState.PartyTurn:
                    _shakeSystem.ShakeObjects(_partyDices);
                    break;
                case TurnState.EnemyTurn:
                    _shakeSystem.ShakeObjects(_enemyDices);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}