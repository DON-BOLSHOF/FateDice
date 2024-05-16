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

        [Inject] private MainHeroHolder _mainHeroHolder;

        public async void ManageWin(Unit[] partyPack)
        {
            var persentageFrom = partyPack.Select(unit => unit.Class.XPPercentage).ToArray();
            
            foreach (var unit in partyPack)
            {
                unit.Class.ModifyXP(_xpAwarding.XPAward/partyPack.Length);
            }
            
            var persentageTo = partyPack.Select(unit => unit.Class.XPPercentage).ToArray();
            
            _updateXpPanel.ActivatePanel(partyPack, persentageFrom, persentageTo);
            await _updateXpPanel.OnCompleted.ToUniTask(useFirstValue: true);
            
            LoadLevel(partyPack);
        }
        
        private void LoadLevel(Unit[] partyPack)
        {
            _levelManager.LoadLevel("WorldMap", container =>
            {
                container.Bind<Unit>().WithId("MainHero").FromInstance(_mainHeroHolder.MainHero).AsSingle();
                
                foreach (var unit in partyPack)
                {
                    container.Bind<Unit>().FromInstance(unit).AsCached();
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