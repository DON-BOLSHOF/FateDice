using System;
using BKA.Buffs;
using BKA.System;
using BKA.UI.WorldMap.Class;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;
using Unit = BKA.Units.Unit;

namespace BKA.WorldMapDirectory
{
    public class ClassHandler : MonoBehaviour
    {
        [SerializeField] private ClassPanel _classPanel;
        [SerializeField] private ClassButton _classButton;

        [Inject] private GameSession _gameSession;

        private void Awake()
        {
            _classPanel.Fullfill(_gameSession.Party);

            _classButton.OnClassButtonClicked.Subscribe(_ => _classPanel.Activate()).AddTo(this);
            _classPanel.OnChooseSpecialization.Subscribe(ModifyClass).AddTo(this);
            _gameSession.OnUnitDataUpdated.Subscribe(unit => _classPanel.UpdateLocalData(unit)).AddTo(this);
            _gameSession.OnUnitHolderUpdated.Subscribe(_ => _classPanel.UpdateHeroesHolder(_gameSession.Party)).AddTo(this);
        }

        private void ModifyClass((Unit, Specialization) valueTuple)
        {
            var unit = valueTuple.Item1;
            var specialization = valueTuple.Item2;

            unit.Class.LevelUp(specialization);
        }
    }
}