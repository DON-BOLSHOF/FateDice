using System.Collections.Generic;
using System.Linq;
using BKA.Buffs;
using BKA.System;
using BKA.System.UploadData;
using BKA.UI;
using BKA.Units;
using Cysharp.Threading.Tasks;
using Zenject;

namespace BKA.BattleDirectory.BattleSystems
{
    public class WinHandler
    {
        [Inject] private LevelManager _levelManager;

        [Inject] private IArtefactAwarding _artefactAwarding;
        [Inject] private IXPAwarding _xpAwarding;

        [Inject] private IUpdateXPPanel _updateXpPanel;

        public async void ManageWin(List<UnitBattleBehaviour> partyPack)
        {
            var persentageFrom = partyPack.Select(unitBehaviour => unitBehaviour.Unit.Class.XPPercentage).ToArray();
            
            foreach (var unitBattleBehaviour in partyPack)
            {
                unitBattleBehaviour.Unit.Class.ModifyXP(_xpAwarding.XPAward/partyPack.Count);
            }
            
            var persentageTo = partyPack.Select(unitBehaviour => unitBehaviour.Unit.Class.XPPercentage).ToArray();
            
            _updateXpPanel.ActivatePanel(partyPack.Select(unitBehaviour => unitBehaviour.Unit).ToArray(), persentageFrom, persentageTo);
            await _updateXpPanel.OnCompleted.ToUniTask(useFirstValue: true);
            
            LoadLevel(partyPack);
        }
        
        private void LoadLevel(List<UnitBattleBehaviour> partyPack)
        {
            _levelManager.LoadLevel("WorldMap", container =>
            {
                foreach (var unitBattleBehaviour in partyPack)
                {
                    container.Bind<Unit>().FromInstance(unitBattleBehaviour.Unit)
                        .AsCached();
                }
                
                foreach (var awardingArtefact in _artefactAwarding.Artefacts)
                {
                    container.Bind<Artefact>().FromInstance(awardingArtefact).AsCached();
                }

                container.Bind<SystemStage>().FromInstance(SystemStage.LocalChanges).AsSingle();
            });
        }
    }
}