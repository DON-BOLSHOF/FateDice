﻿using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Unit = BKA.Units.Unit;

namespace BKA.UI.WorldMap
{
    [RequireComponent(typeof(Button))]
    public class HeroWorldMapWidget : MonoBehaviour
    {
        [SerializeField] private Transform _view;
        [SerializeField] private HealthWidget _healthWidget;
        [SerializeField] private TextMeshProUGUI _heroId;
        [SerializeField] private Image _icon;

        public IObservable<UniRx.Unit> OnSelected => _onSelected;

        public Unit Hero => _hero;
        private Unit _hero;

        private Button _selectedButton;
        private ReactiveCommand _onSelected = new();

        private void Start()
        {
            _selectedButton = GetComponent<Button>();

            _selectedButton.OnClickAsObservable().Subscribe(_ => _onSelected?.Execute()).AddTo(this);
        }

        public void UpdateData(Unit unit)
        {
            _hero = unit;

            _heroId.text = unit.Definition.ID;
            _icon.sprite = unit.Definition.UnitIcon;
            _healthWidget.SetHealth(unit.Health.Value);
        }

        public void UpdateLocalData()
        {
            _healthWidget.SetHealth(_hero.Health.Value);
        }

        public void PutForward()
        {
            _view.localPosition += new Vector3(30f, 0, 0);
        }

        public void PutBase()
        {
            _view.localPosition = Vector3.zero;
        }
    }
}