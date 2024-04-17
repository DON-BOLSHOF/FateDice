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

        private void Start()
        {
            _inventoryPanel.Fullfill(_gameSession.Party, _gameSession.Artefacts);
            
            inventoryButton.OnInventoryWidgetClicked.Subscribe(_ => _inventoryPanel.Activate())
                .AddTo(this);
        }
    }
}