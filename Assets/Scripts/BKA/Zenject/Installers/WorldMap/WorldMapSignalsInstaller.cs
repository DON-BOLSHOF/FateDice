using BKA.Zenject.Signals;
using UnityEngine;

namespace Zenject.WorldMap
{
    [CreateAssetMenu(menuName = "Installers/WorldMap/WorldMapSignalsInstaller", fileName = "WorldMapSignalsInstaller")]
    public class WorldMapSignalsInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            
            Container.DeclareSignal<BlockInputSignal>();

            Container.DeclareSignal<TakeArtefactSignal>();

            Container.DeclareSignal<SFXClipSignal>();

            Container.DeclareSignal<ExtraordinaryBattleSignal>();

            Container.DeclareSignal<UploadNewHeroSignal>();
            
            Container.DeclareSignal<UpdateNewHeroSignal>();

            Container.DeclareSignal<ExtraodinaryDialogActivate>();

            Container.DeclareSignal<GiveXPSignal>();
        }
    }
}