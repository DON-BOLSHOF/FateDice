using BKA.UI.WorldMap.Dialog;
using UnityEngine;

namespace Zenject.WorldMap
{
    [CreateAssetMenu(menuName = "Installers/WorldMap/DialogHandlerInstaller", fileName = "DialogHandlerInstaller")]
    public class DialogHandlerInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private AudioClip _charSpawnedClip;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<DialogHandler>().AsSingle().WithArguments(_charSpawnedClip);
        }
    }
}