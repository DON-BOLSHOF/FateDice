using System;
using System.Linq;
using BKA.System;
using BKA.UI;
using BKA.Zenject.Signals;
using Cysharp.Threading.Tasks;
using Zenject;

namespace BKA.WorldMapDirectory
{
    public class UpdateXPObserver : IDisposable
    {
        private GameSession _gameSession;
        private SignalBus _signalBus;

        private IUpdateXPPanel _updateXpPanel;
        
        public UpdateXPObserver(SignalBus signalBus, GameSession gameSession, IUpdateXPPanel updateXpPanel)
        {
            _signalBus = signalBus;
            _gameSession = gameSession;

            _updateXpPanel = updateXpPanel;
            
            _signalBus.Subscribe<GiveXPSignal>(GivePartyXp);
        }
        
        private async void GivePartyXp(GiveXPSignal giveXpSignal)
        {
            _signalBus.Fire(new BlockInputSignal{IsBlocked = true});
            
            var persentageFrom = _gameSession.Party.Select(unit => unit.Class.XPPercentage).ToArray();
            
            foreach (var unit in _gameSession.Party)
            {
                unit.Class.ModifyXP(giveXpSignal.XP/_gameSession.Party.Count);
            }
            
            var persentageTo = _gameSession.Party.Select(unit => unit.Class.XPPercentage).ToArray();

            _updateXpPanel.ActivatePanel(_gameSession.Party.ToArray(), persentageFrom, persentageTo);

            await _updateXpPanel.OnCompleted.ToUniTask(useFirstValue: true);
            
            _signalBus.Fire(new BlockInputSignal{IsBlocked = false});
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<GiveXPSignal>(GivePartyXp);
            _gameSession?.Dispose();
        }
    }
}