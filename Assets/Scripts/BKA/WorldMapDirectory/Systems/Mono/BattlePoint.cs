using System;
using System.Collections.Generic;
using BKA.Units;
using UniRx;
using UnityEngine;

namespace BKA.WorldMapDirectory.Systems
{
    [Serializable]
    public class BattlePointData
    {
        public bool IsBattleBegan;
    }
    
    public abstract class BattlePoint : MonoBehaviour, IDisposable
    {
        [SerializeField] private UnitDefinition[] _unitDefinitions;
        [SerializeField] private int _battleXPValue;

        public IObservable<(IEnumerable<UnitDefinition>, int)> OnBattleStart => _onBattleStart;
        public BattlePointData BattlePointData => _battlePointData;

        protected readonly ReactiveCommand<(IEnumerable<UnitDefinition>, int)> _onBattleStart = new();
        protected CompositeDisposable _pointDisposable = new();

        private BattlePointData _battlePointData = new();

        public void DynamicInit(BattlePointData battlePointData)
        {
            _battlePointData = battlePointData;

            if (_battlePointData.IsBattleBegan)
            {
                Dispose();
                Destroy(this);
            }
        }

        public abstract void Dispose();

        protected void StartBattle()
        {
            _battlePointData.IsBattleBegan = true;
            _onBattleStart?.Execute((_unitDefinitions, _battleXPValue));
        }
    }
}