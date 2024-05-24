using System;
using BKA.UI;
using BKA.Zenject.Signals;
using UniRx;
using UnityEngine;
using Zenject;

namespace BKA.System
{
    public class GameFinilizerHandler : IDisposable
    {
        private GameFinilizerPanel _gameFinilizerPanel;

        private SignalBus _signalBus;
        
        private CompositeDisposable _handlerDisposable = new();
        
        public GameFinilizerHandler(GameFinilizerPanel gameFinilizerPanel, SignalBus signalBus)
        {
            _gameFinilizerPanel = gameFinilizerPanel;

            _signalBus = signalBus;
            
            _signalBus.Subscribe<MainQuestEndSignal>(ActivateEndOfMainQuest);

            _gameFinilizerPanel.OnFinishedGame.Subscribe(_ => FinishGame()).AddTo(_handlerDisposable);
        }

        private void ActivateEndOfMainQuest(MainQuestEndSignal mainQuestEndSignal)
        {
            _gameFinilizerPanel.Activate(true);
        }

        private void FinishGame()
        {
            Application.Quit();
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<MainQuestEndSignal>(ActivateEndOfMainQuest);
            _handlerDisposable?.Dispose();
        }
    }
}