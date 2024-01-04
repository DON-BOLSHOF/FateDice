using System;
using BKA.Units;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace BKA
{
    public class BootsTrap : MonoBehaviour
    {
        [Inject] private ZenjectSceneLoader _sceneLoader;
        
        [Inject] private DefinitionPool _definitionPool;

        private async void Start()
        {
            await _definitionPool.UploadBaseDefinitions();

            _sceneLoader.LoadScene("BattleScene", LoadSceneMode.Single ,(container) =>
            {
                container.Bind<Unit[]>().WithId("Teammates").FromInstance(new Unit[]{new DemonPaladin(_definitionPool)}).AsCached()
                    .WhenInjectedInto<BattleEntryPoint>();
                container.Bind<Unit[]>().WithId("Enemies").FromInstance(new Unit[] { new DemonPaladin(_definitionPool) }).AsCached()
                    .WhenInjectedInto<BattleEntryPoint>();
            });
        }
    }
}