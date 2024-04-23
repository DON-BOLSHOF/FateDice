using BKA.WorldMapDirectory.Artefacts;
using UnityEngine;
using Zenject;

namespace BKA.System
{
    public class FakeArtefactSpawner : MonoBehaviour
    {
        [SerializeField] private Artefact _artefact;

        [Inject] private GameSession _gameSession;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _gameSession.RemoveArtefact(_artefact);
            }
        }
    }
}