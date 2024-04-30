using System;
using System.Linq;
using BKA.Buffs;
using ModestTree;
using UniRx;
using UnityEngine;

namespace BKA.UI
{
    public class PerkHolder : MonoBehaviour
    {
        [SerializeField] private PerkWidget[] _perkWidgets;

        public IObservable<Specialization> OnPerkSelected => _onPerkSelected;

        private readonly ReactiveCommand<Specialization> _onPerkSelected = new();

        private void Start()
        {
            foreach (var perkWidget in _perkWidgets)
            {
                perkWidget.OnSelectedPerk.Subscribe(specialization =>
                    {
                        perkWidget.SelectView();
                        
                        foreach (var widget in _perkWidgets.Where(widget => widget != perkWidget))
                        {
                            widget.UnSelectView();
                        }
                        
                        _onPerkSelected?.Execute(specialization);
                    })
                    .AddTo(this);
            }
        }

        public void UpdateData(Specialization[] specializations)
        {
            if (specializations.Length > _perkWidgets.Length)
                throw new ApplicationException("Too many specialization");

            var index = 0;

            for (index = 0; index < specializations.Length; index++)
            {
                _perkWidgets[index].UpdateData(specializations[index]);
                _perkWidgets[index].gameObject.SetActive(true);
            }

            for (; index < _perkWidgets.Length; index++)
            {
                _perkWidgets[index].gameObject.SetActive(false);
            }
        }

        public void UpdateData(Specialization[] specializations, Specialization selectedOne)
        {
            UpdateData(specializations);

            var indexOf = specializations.IndexOf(selectedOne);
            _perkWidgets[indexOf].SelectView();
        }

        public void FormingHint(Specialization[] hintSpecialization)
        {
            UpdateData(hintSpecialization);
            foreach (var perkWidget in _perkWidgets)
            {
                perkWidget.ActivateHint();
            }
        }

        public void DeactivateHint()
        {
            foreach (var perkWidget in _perkWidgets)
            {
                perkWidget.DeactivateHint();
            } 
        }

        public void LockInput(bool value)
        {
            foreach (var perkWidget in _perkWidgets)
            {
                perkWidget.LockInput(value);
            }
        }

        public void ClearData()
        {
            foreach (var perkWidget in _perkWidgets)
            {
                perkWidget.UnSelectView();
                perkWidget.ClearData();
                perkWidget.gameObject.SetActive(true);
            }
        }
    }
}