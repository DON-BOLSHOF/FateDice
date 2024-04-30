using System;
using System.Collections.Generic;
using System.Linq;
using BKA.Units;
using UniRx;
using UnityEngine;
using Zenject;
using Unit = BKA.Units.Unit;

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

        [Inject] private UnitFactory _unitFactory;

        public IObservable<(IEnumerable<Unit>, int)> OnBattleStart => _onBattleStart;
        public BattlePointData BattlePointData => _battlePointData;

        private readonly ReactiveCommand<(IEnumerable<Unit>, int)> _onBattleStart = new();

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
            _onBattleStart?.Execute((_unitDefinitions.Select(definition => _unitFactory.UploadUnit(definition)), _battleXPValue));
        }
    }
}