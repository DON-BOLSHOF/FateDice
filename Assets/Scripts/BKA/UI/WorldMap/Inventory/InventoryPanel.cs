using System;
using System.Collections.Generic;
using System.Linq;
using BKA.UI.WorldMap;
using BKA.UI.WorldMap.Inventory;
using BKA.WorldMapDirectory.Artefacts;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Unit = BKA.Units.Unit;

namespace BKA.UI.Inventory
{
    public class InventoryPanel : MonoBehaviour
    {
        [SerializeField] private Button _exitButton;

        [SerializeField] private Transform _view;

        [SerializeField] private HeroInventoryWidget[] _heroInventoryWidgets;

        [SerializeField] private Sweep _sweep;

        [SerializeField] private ItemHolder _itemHolder;

        [SerializeField] private HeroArtefactWidget _heroArtefactWidget;

        public IObservable<List<Artefact>> OnUpdatedInventory => _onUpdatedInventory;

        private ReactiveProperty<HeroInventoryWidget> _selectedWidget = new();
        private ReactiveCommand<List<Artefact>> _onUpdatedInventory = new();

        private void Start()
        {
            _exitButton.OnClickAsObservable().Subscribe(_ => _view.gameObject.SetActive(false)).AddTo(this);

            _selectedWidget.Skip(1).Subscribe(ChangeSelectedHero).AddTo(this);

            foreach (var heroInventoryWidget in _heroInventoryWidgets)
            {
                heroInventoryWidget.OnSelected.Subscribe(_ => _selectedWidget.Value = heroInventoryWidget).AddTo(this);
            }

            _heroArtefactWidget.OnUpdatedActions.Subscribe(
                _ => _sweep.UpdateData(_selectedWidget.Value.Hero.DiceActions)
            ).AddTo(this);

            _itemHolder.OnUpdatedData.Subscribe(_ => _onUpdatedInventory.Execute(_itemHolder.GetArtefacts()))
                .AddTo(this);
            _heroArtefactWidget.OnUpdatedData.Subscribe(_ => _onUpdatedInventory.Execute(_itemHolder.GetArtefacts()))
                .AddTo(this);

            _selectedWidget.Value = _heroInventoryWidgets[0];
        }

        public void Activate()
        {
            _view.gameObject.SetActive(true);
        }

        public void Fullfill(List<Unit> gameSessionParty, List<Artefact> partyArtefacts)
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

            _itemHolder.SetArtefacts(partyArtefacts);
        }

        public void UpdateArtefacts(List<Artefact> gameSessionArtefacts)
        {
            var localArtefacts = _itemHolder.GetArtefacts();
            
            foreach (var artefact in gameSessionArtefacts)
            {
                if (localArtefacts.Find(artef => artef.Equals(artefact)))
                {
                    localArtefacts.Remove(artefact);
                }
                else
                {
                    _itemHolder.UploadArtefact(artefact);
                }
            }

            foreach (var localArtefact in localArtefacts)
            {
                _itemHolder.ClearArtefact(localArtefact);
            }

        }

        private void ChangeSelectedHero(HeroInventoryWidget heroInventoryWidget)
        {
            _sweep.UpdateData(heroInventoryWidget.Hero.DiceActions);
            _heroArtefactWidget.SetHero(heroInventoryWidget.Hero);
            heroInventoryWidget.PutForward();

            foreach (var inventoryWidget in _heroInventoryWidgets.Where(value => !heroInventoryWidget.Equals(value)))
            {
                inventoryWidget.PutBase();
            }
        }
    }
}