using System;
using BKA.System;
using BKA.UI.WorldMap.Class;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;

namespace BKA.WorldMapDirectory.Class
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
        }
    }
}