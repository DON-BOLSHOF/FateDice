using System;
using System.Collections.Generic;
using System.Linq;
using BKA.Buffs;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Unit = BKA.Units.Unit;

namespace BKA.UI.WorldMap.Class
{
    public class ClassPanel : MonoBehaviour
    {
        [SerializeField] private Button _exitButton;

        [SerializeField] private Transform _view;

        [SerializeField] private List<HeroWorldMapWidget> _heroWorldMapWidgets;

        [SerializeField] private UpgradePanel _upgradePanel;

        public IObservable<(Unit, Specialization)> OnChooseSpecialization => _upgradePanel.OnChooseSpecialization;

        private ReactiveProperty<HeroWorldMapWidget> _selectedWidget = new();

        private void Start()
        {
            _exitButton.OnClickAsObservable().Subscribe(_ => _view.gameObject.SetActive(false)).AddTo(this);

            _selectedWidget.Skip(1).Subscribe(ChangeSelectedHero).AddTo(this);

            foreach (var heroInventoryWidget in _heroWorldMapWidgets)
            {
                heroInventoryWidget.OnSelected.Subscribe(_ => _selectedWidget.Value = heroInventoryWidget).AddTo(this);
            }

            _selectedWidget.Value = _heroWorldMapWidgets[0];
        }

        public void Activate()
        {
            _view.gameObject.SetActive(true);
        }

        public void Fullfill(List<Unit> gameSessionParty)
        {
            if (_heroWorldMapWidgets.Count < gameSessionParty.Count)
                throw new ArgumentException("Слишком много в партии игрока персонажей");

            var i = 0;

            for (i = 0; i < gameSessionParty.Count; i++)
            {
                _heroWorldMapWidgets[i].UpdateData(gameSessionParty[i]);
                _heroWorldMapWidgets[i].gameObject.SetActive(true);
            }

            for (; i < _heroWorldMapWidgets.Count; i++)
            {
                _heroWorldMapWidgets[i].gameObject.SetActive(false);
            }
        }
        
        public void UpdateHeroesHolder(List<Unit> gameSessionParty)
        {
            if (_heroWorldMapWidgets.Count < gameSessionParty.Count)
                throw new ArgumentException("Слишком много в партии игрока персонажей");

            var i = 0;

            for (i = 0; i < gameSessionParty.Count; i++)
            {
                if (_heroWorldMapWidgets[i].Hero == null)
                {
                    _heroWorldMapWidgets[i].UpdateData(gameSessionParty[i]);
                    _heroWorldMapWidgets[i].gameObject.SetActive(true);
                }
                else if (_heroWorldMapWidgets[i].Hero != gameSessionParty[i])
                {
                    _heroWorldMapWidgets[i].UpdateData(null);
                    _heroWorldMapWidgets[i].gameObject.SetActive(false);
                    
                    _heroWorldMapWidgets.Insert(_heroWorldMapWidgets.Count, _heroWorldMapWidgets[i]);
                    _heroWorldMapWidgets.RemoveAt(i);
                    i--;
                }
            }

            for (; i < _heroWorldMapWidgets.Count; i++)
            {
                _heroWorldMapWidgets[i].gameObject.SetActive(false);
            }
        }

        public void UpdateLocalData(Unit unit)
        {
            var heroWorldMapWidget = _heroWorldMapWidgets.First(widget => widget.Hero.Equals(unit));
            heroWorldMapWidget.UpdateLocalData();

            if (_selectedWidget.Value.Hero.Equals(unit))
                _upgradePanel.UpdateLocalData();
        }

        private void ChangeSelectedHero(HeroWorldMapWidget heroWorldMapWidget)
        {
            _upgradePanel.UpdateData(heroWorldMapWidget.Hero);

            heroWorldMapWidget.PutForward();
            foreach (var inventoryWidget in _heroWorldMapWidgets.Where(value => !heroWorldMapWidget.Equals(value)))
            {
                inventoryWidget.PutBase();
            }
        }
    }
}