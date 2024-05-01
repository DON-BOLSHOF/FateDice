using System;
using BKA.Buffs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace BKA.UI.WorldMap.Class
{
    public class UpgradePanel : MonoBehaviour
    {
        [SerializeField] private Image _icon;

        [SerializeField] private Sweep _sweep;
        [SerializeField] private SpecializationPanel _specializationPanel;

        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _definition;

        [SerializeField] private Slider _xpSlider;

        [SerializeField] private CharacteristicPanel _characteristicPanel;

        public IObservable<(Units.Unit, Specialization)> OnChooseSpecialization => _onChooseSpecialization;

        private ReactiveCommand<(Units.Unit, Specialization)> _onChooseSpecialization = new();
        private Units.Unit _currentHero;

        private void Start()
        {
            _specializationPanel.OnSpecializationChoose.Subscribe(specialization =>
                _onChooseSpecialization?.Execute((_currentHero, specialization))).AddTo(this);
            _specializationPanel.OnSpecializationSelected.Subscribe(specialization =>
            {
                if (specialization != null)
                {
                    _characteristicPanel.MakeHint(specialization.Characteristics);
                    _sweep.MakeHint(specialization.DiceActionPairs);
                }
                else
                {
                    _sweep.DeactivateHint();
                    _characteristicPanel.DeactivateHint();
                }
            }).AddTo(this);
        }

        public void UpdateData(Units.Unit currentHero)
        {
            _currentHero = currentHero;

            _icon.sprite = currentHero.Definition.UnitIcon;
            _name.text = currentHero.Definition.ID;
            _definition.text = currentHero.Definition.UnitDescription;

            _xpSlider.value = currentHero.Class.XPPercentage;
            _characteristicPanel.UpdateData(currentHero.Class.Characteristics);

            _sweep.UpdateData(currentHero);
            _specializationPanel.UpdateData(currentHero.Class);
        }

        public void UpdateLocalData()
        {
            _characteristicPanel.UpdateData(_currentHero.Class.Characteristics);
            _sweep.UpdateData(_currentHero);
            _xpSlider.value = _currentHero.Class.XPPercentage;
            _specializationPanel.UpdateLocalData();
        }
    }
}