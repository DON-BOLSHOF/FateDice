using BKA.System;
using BKA.Units;
using UnityEngine;
using Zenject;

namespace BKA.BootsTraps
{
    public class BootsTrap : MonoBehaviour
    {
        [Inject] private DefinitionPool _definitionPool;

        [Inject] private BootsTrapStateObserver _stateObserver;
        
        [Inject] private LevelManager _levelManager;

        public async void Start()
        {
            if(_stateObserver.BootsTrapState.Value == BootsTrapState.Loaded)
                return;
            
            await _definitionPool.UploadBaseDefinitions();
            
            _stateObserver.Visit(this);
            
            _levelManager.LoadLevel("BattleScene", (container) =>
            {
                container.Bind<Unit[]>().WithId("Party").FromInstance(new Unit[]{new DemonPaladin(_definitionPool), new DemonPaladin(_definitionPool)}).AsCached();
                container.Bind<Unit[]>().WithId("Enemies").FromInstance(new Unit[]{new DemonPaladin(_definitionPool)}).AsCached();
            });
        }
    }
}