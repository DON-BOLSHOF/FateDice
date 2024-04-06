﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BKA.Dices.DiceActions;
using BKA.UI;
using BKA.Units;
using Cysharp.Threading.Tasks;
using Sirenix.Utilities;
using UniRx;
using UnityEngine;

namespace BKA.BattleDirectory.PlayerInput
{
    public class BattleInputHandler : MonoBehaviour
    {
        [SerializeField] private CharacterPanel[] _characterPanels;

        private bool _isMakeTurn;

        private ReactiveProperty<UnitBattleBehaviour> _turningUnit = new();

        private List<UnitBattleBehaviour> _party;

        private CancellationTokenSource _cancellationTokenSource = new();

        private void Start()
        {
            foreach (var characterPanel in _characterPanels)
            {
                characterPanel.OnPanelClicked.Subscribe(OnUnitBehaviourClicked).AddTo(this);
            }

            _turningUnit.Skip(1).Subscribe(value =>
            {
                var characterPanel = _characterPanels.First(panel => panel.UnitBattleBehaviour == value);
                characterPanel.SetActing();

                _characterPanels.Where(panel => panel != characterPanel)
                    .ForEach(panel => panel.SetUnActing());
            }).AddTo(this);
        }

        public async UniTask MakeTurn(List<UnitBattleBehaviour> party, CancellationToken token)
        {
            _party = party;

            _isMakeTurn = true;

            foreach (var unitBattleBehaviour in party.Where(unitBattleBehaviour => unitBattleBehaviour
                         .DiceAction.DiceActionData.DiceAttributeFocus == DiceAttributeFocus.None))
            {
                unitBattleBehaviour.Act(token).Forget();
            }
            
            var uniTasks = party.Select(unit => unit.IsReadyToAct.Where(value => !value)
                .ToUniTask(useFirstValue:true, cancellationToken: token));
            await UniTask.WhenAll(uniTasks);

            _isMakeTurn = false;
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

            if (_turningUnit.Value.DiceAction.DiceActionData.DiceAttributeFocus == DiceAttributeFocus.Ally &&
                !_party.Contains(unit) && unit.IsReadyToAct.Value)
            {
                _turningUnit.Value = unit;
                return;
            }

            if (_turningUnit.Value == unit)
            {
                _turningUnit.Value = null;
                return;
            }

            if (!_turningUnit.Value.IsReadyToAct.Value)
            {
                return;
            }

            switch (_turningUnit.Value.DiceAction.DiceActionData.DiceAttributeFocus)
            {
                case DiceAttributeFocus.Enemy when
                    !_party.Contains(unit):
                    _turningUnit.Value.DiceAction.ChooseTarget(unit);
                    break;
                case DiceAttributeFocus.Ally when
                    _party.Contains(unit):
                    _turningUnit.Value.DiceAction.ChooseTarget(unit);
                    break;
                case DiceAttributeFocus.None:
                    break;
                default:
                    return;
            }

            _turningUnit.Value.Act(_cancellationTokenSource.Token).Forget();
            _turningUnit.Value = null;
        }

        private void OnDestroy()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }
    }
}