using UnityEngine;

namespace BKA.Audio.Mono
{
    public class MainAudioAmbient : MonoBehaviour
    {
        private static MainAudioAmbient _mainAudioAmbient;
        
        private void Awake()
        {
            if (_mainAudioAmbient != null)
            {
                Destroy(gameObject);
            }
            else
            {
                _mainAudioAmbient = this;
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}