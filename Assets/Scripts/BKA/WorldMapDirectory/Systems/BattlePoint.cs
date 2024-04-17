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
    [RequireComponent(typeof(InteractableObject))]
    public class BattlePoint : MonoBehaviour
    {
        [SerializeField] private UnitDefinition[] _unitDefinitions;

        [Inject] private UnitFactory _unitFactory;

        public IObservable<IEnumerable<Unit>> OnBattleStart => _onBattleStart;

        private readonly ReactiveCommand<IEnumerable<Unit>> _onBattleStart = new();

        private InteractableObject _interactableObject;

        private void Start()
        {
            _interactableObject = GetComponent<InteractableObject>();
            
            _interactableObject.OnInteracted.Subscribe(_ => StartBattle()).AddTo(this);
        }

        private void StartBattle()
        {
            _onBattleStart?.Execute(_unitDefinitions.Select(definition => _unitFactory.UploadUnit(definition)));
        }
    }
}