using System.Collections.Generic;
using BKA.Buffs;
using BKA.System;
using BKA.TestUploadData;
using BKA.Units;
using Zenject;

namespace BKA.BattleDirectory.BattleSystems
{
    public class WinHandler
    {
        [Inject] private LevelManager _levelManager;

        [Inject] private IArtefactAwarding _artefactAwarding;

        public void ManageWin(List<UnitBattleBehaviour> partyPack)
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