using BKA.System;
using BKA.UI.Inventory;
using UniRx;
using UnityEngine;
using Zenject;

namespace BKA.WorldMapDirectory.Inventory
{
    public class InventoryHandler : MonoBehaviour
    {
        [SerializeField] private InventoryPanel _inventoryPanel;
        [SerializeField] private InventoryButton inventoryButton;

        [Inject] private GameSession _gameSession;

        private void Awake()
        {
            _inventoryPanel.Fullfill(_gameSession.Party, _gameSession.Artefacts);
            
            inventoryButton.OnInventoryWidgetClicked.Subscribe(_ => _inventoryPanel.Activate())
                .AddTo(this);

            _inventoryPanel.OnUpdatedInventory.Subscribe(artefacts =>
                _gameSession.UpdateArtefacts(artefacts)).AddTo(this);

            _gameSession.OnArtefactsUpdated.Subscribe(_ => _inventoryPanel.UpdateArtefacts(_gameSession.Artefacts)).AddTo(this);
        }
    }
}