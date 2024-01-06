using System;
using ModestTree.Util;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace BKA.System
{
    public class LevelManager : MonoBehaviour
    {
        [Inject] private ZenjectSceneLoader _sceneLoader;
        
        public void LoadLevel(string levelName)
        {
            SceneManager.LoadScene(levelName);
        }

        public void LoadLevel(string levelName, Action<DiContainer> containerAction)
        {
            _sceneLoader.LoadScene(levelName, LoadSceneMode.Single, containerAction);
        }
    }
}