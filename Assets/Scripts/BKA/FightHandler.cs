using System;
using System.Collections.Generic;
using BKA.BattleDirectory;
using BKA.Units;
using UnityEngine;
using Zenject;

namespace BKA
{
    public class FightHandler : MonoBehaviour, ITurnSystemVisitor
    {
        private List<UnitBattleBehaviour> _firstPack;
        private List<UnitBattleBehaviour> _secondPack;

        [Inject] private TurnSystem _turnSystem;

        public void DynamicInit(List<UnitBattleBehaviour> teammates, List<UnitBattleBehaviour> enemy)
        {
            _firstPack = teammates;
            _secondPack = enemy;
        }

        public void StartBattle()
        {
        }

        public void Accept(TurnSystem turnSystem)
        {
            turnSystem.Visit(this);
        }
    }
}
