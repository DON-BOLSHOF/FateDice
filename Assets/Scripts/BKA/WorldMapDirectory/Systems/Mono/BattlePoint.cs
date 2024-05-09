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
    
    [RequireComponent(typeof(InteractableObject))]
    public class BattlePoint : MonoBehaviour, IDisposable
    {
        [SerializeField] private UnitDefinition[] _unitDefinitions;
        [SerializeField] private int _battleXPValue;

        public IObservable<(IEnumerable<UnitDefinition>, int)> OnBattleStart => _onBattleStart;
        public BattlePointData BattlePointData => _battlePointData;

        private readonly ReactiveCommand<(IEnumerable<UnitDefinition>, int)> _onBattleStart = new();

        private InteractableObject _interactableObject;

        private BattlePointData _battlePointData = new();

        private CompositeDisposable _pointDisposable = new();

        private void Start()
        {
            _interactableObject = GetComponent<InteractableObject>();
            
            _interactableObject.OnInteracted.Subscribe(_ => StartBattle()).AddTo(_pointDisposable);
        }

        public void DynamicInit(BattlePointData battlePointData)
        {
            _battlePointData = battlePointData;

            if (_battlePointData.IsBattleBegan)
            {
                Dispose();
                Destroy(this);
            }
        }

        public void Dispose()
        {
            _onBattleStart?.Dispose();
            _pointDisposable?.Dispose();
        }

        private void StartBattle()
        {
            _battlePointData.IsBattleBegan = true;
            _onBattleStart?.Execute((_unitDefinitions, _battleXPValue));
        }
    }
}