using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BKA.BattleDirectory.BattleSystems;
using BKA.Dices.DiceActions;
using BKA.System.Exceptions;
using BKA.UI;
using BKA.Units;
using Cysharp.Threading.Tasks;
using Sirenix.Utilities;
using UniRx;
using UnityEngine;
using Zenject;

namespace BKA.BattleDirectory.PlayerInput
{
    public class BattleInputHandler : MonoBehaviour
    {
        [SerializeField] private PartyCharacterPanel[] _partyCharacterPanels;

        [SerializeField] private CharacterPanel[] _enemyCharacterPanels;

        [Inject] private TurnSystem _turnSystem;// Полный пиздец потом какую-то другую проверку сделать, КОСТЫЛЬ

        private bool _isMakeTurn;

        private ReactiveProperty<UnitBattleBehaviour> _turningUnit = new();

        private List<UnitBattleBehaviour> _party;

        private CancellationTokenSource _cancellationTokenSource = new();

        private readonly Stack<UnitBattleBehaviour> _actedUnits = new();

        public bool HasToUndo => _actedUnits.Count > 0;

        private void Start()
        {
            foreach (var characterPanel in _partyCharacterPanels)
            {
                characterPanel.OnPanelClicked.Subscribe(OnUnitBehaviourClicked).AddTo(this);
            }

            foreach (var enemyCharacterPanel in _enemyCharacterPanels)
            {
                enemyCharacterPanel.OnPanelClicked.Subscribe(OnUnitBehaviourClicked).AddTo(this);
            }

            _turnSystem.TurnState.Where(value => value == TurnState.EnemyTurn).Subscribe(_ =>
            {
                foreach (var partyCharacterPanel in _partyCharacterPanels)
                {
                    partyCharacterPanel.SetUnActing();
                }
            }).AddTo(this);

            _turningUnit.Skip(1).Subscribe(value =>
            {
                var characterPanel = _partyCharacterPanels.First(panel => panel.UnitBattleBehaviour == value);
                characterPanel.SetActing();

                _partyCharacterPanels.Where(panel => panel != characterPanel)
                    .ForEach(panel => panel.SetUnActing());
            }).AddTo(this);
        }

        public async UniTask MakeTurn(List<UnitBattleBehaviour> party, CancellationToken token)
        {
            _party = party;

            _actedUnits.Clear();
            _isMakeTurn = true;

            foreach (var unitBattleBehaviour in party.Where(unitBattleBehaviour =>
                         unitBattleBehaviour.DiceAction.DiceActionData.DiceAttributeFocus == DiceAttributeFocus.None))
            {
                unitBattleBehaviour.Act();
            }

            await UniTask.WaitUntil(() => party.Count(unit =>
                unit.IsActed.Value) == party.Count, cancellationToken: token).WithPostCancellation(() =>
            {
                if (_turningUnit.Value != null)
                {
                    var characterPanel = _partyCharacterPanels.First(panel => panel.UnitBattleBehaviour == _turningUnit.Value);
                    characterPanel.SetUnActing();
                }
                
                _isMakeTurn = false;
            });

            _isMakeTurn = false;
        }

        public void UndoLastAct()
        {
            if (_actedUnits.Count <= 0)
                throw new ApplicationException("Пытаешься отменить то чего нет");

            var actedUnit = _actedUnits.Pop();
            actedUnit.UndoAct();
        }

        private void OnUnitBehaviourClicked(UnitBattleBehaviour unit)
        {
            if (!_isMakeTurn)
                return;

            switch (_turningUnit.Value)
            {
                case null when unit.IsReadyToAct.Value && _party.Contains(unit):
                    _turningUnit.Value = unit;
                    return;
                case null:
                    return;
            }

            if (_turningUnit.Value != null)
            {
                switch (_turningUnit.Value.DiceAction.DiceActionData.DiceAttributeFocus)
                {
                    case DiceAttributeFocus.Enemy when
                        !_party.Contains(unit) && _turningUnit.Value.IsReadyToAct.Value:
                        _turningUnit.Value.DiceAction.ChooseTarget(unit);

                        _turningUnit.Value.Act();
                        _actedUnits.Push(_turningUnit.Value);
                        _turningUnit.Value = null;
                        return;
                    case DiceAttributeFocus.Ally when
                        _party.Contains(unit) && _turningUnit.Value.IsReadyToAct.Value:
                        _turningUnit.Value.DiceAction.ChooseTarget(unit);

                        _turningUnit.Value.Act();
                        _actedUnits.Push(_turningUnit.Value);
                        _turningUnit.Value = null;
                        return;
                }
            }

            if (_turningUnit.Value == unit)
            {
                _turningUnit.Value = null;
                return;
            }

            if (_turningUnit.Value.DiceAction.DiceActionData.DiceAttributeFocus != DiceAttributeFocus.Ally &&
                _party.Contains(unit) && unit.IsReadyToAct.Value)
            {
                _turningUnit.Value = unit;
            }
        }

        private void OnDestroy()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }
    }
}