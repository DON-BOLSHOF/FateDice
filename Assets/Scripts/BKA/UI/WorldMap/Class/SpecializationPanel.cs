using System;
using BKA.Buffs;
using BKA.Units;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BKA.UI
{
    public class SpecializationPanel : MonoBehaviour
    {
        [SerializeField] private PerkHolder[] _perkHolders;
        [SerializeField] private Button _chooseButton;

        public IObservable<Specialization> OnSpecializationChoose => _onSpecializationChoose;
        public IReadOnlyReactiveProperty<Specialization> OnSpecializationSelected => _chosenSpecialization;

        private readonly ReactiveProperty<Class> _currentClass = new();
        private SpecializationTree _specializationTree;

        private readonly ReactiveProperty<Specialization> _chosenSpecialization = new();
        private readonly ReactiveCommand<Specialization> _onSpecializationChoose = new();

        [Inject]
        public void Construct(SpecializationIdentifier specializationIdentifier)
        {
            _specializationTree = new SpecializationTree(specializationIdentifier);
        }

        private void Start()
        {
            _currentClass.Subscribe(CheckPossibilityToLevelUp).AddTo(this);

            for (var i = 0; i < _perkHolders.Length; i++)
            {
                var indexPinchedHolder = i;
                _perkHolders[i].OnPerkSelected
                    .Subscribe(specialization =>
                    {
                        _chosenSpecialization.Value = specialization;
                        TryCreateHintSpecializations(_chosenSpecialization.Value, indexPinchedHolder);
                    }).AddTo(this);
            }

            _chooseButton.OnClickAsObservable().Where(_ => _chosenSpecialization.Value != null)
                .Subscribe(_ =>
                {
                    UpdateNextSpecializationStep(_chosenSpecialization.Value);
                    _onSpecializationChoose?.Execute(_chosenSpecialization.Value);
                    _chosenSpecialization.Value = null;
                })
                .AddTo(this);
        }

        public void UpdateData(Class currentHeroClass)
        {
            _currentClass.Value = currentHeroClass;

            _chosenSpecialization.Value = null;

            _specializationTree.FormTree(currentHeroClass);

            UpdateUI();
        }

        public void UpdateLocalData()
        {
            CheckPossibilityToLevelUp(_currentClass.Value);
        }

        private void CheckPossibilityToLevelUp(Class heroClass)
        {
            _chooseButton.enabled = heroClass.OnReadyToLevelUp.Value;
        }

        private void TryCreateHintSpecializations(Specialization specialization, int indexPinchedHolder)
        {
            if (indexPinchedHolder + 1 >= _perkHolders.Length)
                return;

            var hintSpecialization = _specializationTree.GetNextSpecializations(specialization);

            if (hintSpecialization.Length > 0)
            {
                _perkHolders[indexPinchedHolder + 1].FormingHint(hintSpecialization);
            }
            else
            {
                _perkHolders[indexPinchedHolder + 1].ClearData();
            }
        }

        private void UpdateNextSpecializationStep(Specialization chosenSpecialization)
        {
            _specializationTree.MakeNextStep(chosenSpecialization);

            UpdateUI();
        }

        private void UpdateUI()
        {
            foreach (var perkHolder in _perkHolders)
            {
                perkHolder.DeactivateHint();
            }

            var steps = _specializationTree.GetSpecializationPath();
            var index = 0;

            for (; index < steps.Count; index++)
            {
                _perkHolders[index].UpdateData(steps[index].Siblings.ToArray(), steps[index].SelectedSpecialization);
                _perkHolders[index].LockInput(true);
            }

            var specializations = _specializationTree.GetNextSpecializations();

            if (specializations.Length > 0)
            {
                _perkHolders[index].UpdateData(specializations);
                _perkHolders[index].LockInput(false);
                index++;
            }

            for (; index < _perkHolders.Length; index++)
            {
                _perkHolders[index].ClearData();
                _perkHolders[index].LockInput(true);
            }
        }
    }
}