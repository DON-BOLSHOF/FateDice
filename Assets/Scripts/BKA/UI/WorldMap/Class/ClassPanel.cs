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
        
        [SerializeField] private HeroInventoryWidget[] _heroInventoryWidgets;

        [SerializeField] private UpgradePanel _upgradePanel;

        public IObservable<(Unit, Specialization)> OnChooseSpecialization => _upgradePanel.OnChooseSpecialization;
        
        private ReactiveProperty<HeroInventoryWidget> _selectedWidget = new();
        
        private void Start()
        {
            _exitButton.OnClickAsObservable().Subscribe(_ => _view.gameObject.SetActive(false)).AddTo(this);
            
            _selectedWidget.Skip(1).Subscribe(ChangeSelectedHero).AddTo(this);

            foreach (var heroInventoryWidget in _heroInventoryWidgets)
            {
                heroInventoryWidget.OnSelected.Subscribe(_ => _selectedWidget.Value = heroInventoryWidget).AddTo(this);
            }
            
            _selectedWidget.Value = _heroInventoryWidgets[0];
        }

        public void Activate()
        {
            _view.gameObject.SetActive(true);
        }

        public void Fullfill(List<Unit> gameSessionParty)
        {
            if (_heroInventoryWidgets.Length < gameSessionParty.Count)
                throw new ArgumentException("Слишком много в партии игрока персонажей");

            var i = 0;

            for (i = 0; i < gameSessionParty.Count; i++)
            {
                _heroInventoryWidgets[i].UpdateData(gameSessionParty[i]);
                _heroInventoryWidgets[i].gameObject.SetActive(true);
            }

            for (; i < _heroInventoryWidgets.Length; i++)
            {
                _heroInventoryWidgets[i].gameObject.SetActive(false);
            }
        }

        public void UpdateLocalData(Unit unit)
        {
            if(!_selectedWidget.Value.Hero.Equals(unit))
                return;
            
            _upgradePanel.UpdateLocalData();
        }

        private void ChangeSelectedHero(HeroInventoryWidget heroInventoryWidget)
        {
            _upgradePanel.UpdateData(heroInventoryWidget.Hero);
            
            heroInventoryWidget.PutForward();
            foreach (var inventoryWidget in _heroInventoryWidgets.Where(value => !heroInventoryWidget.Equals(value)))
            {
                inventoryWidget.PutBase();
            }
        }
    }
}