using BKA.System.ExtraDirectory;
using BKA.Units;
using UnityEngine;
using Zenject;

namespace BKA.BattleDirectory.BattleSystems
{
    public class MainHeroHolder : MonoBehaviour
    {
        [InjectOptional(Id = "MainHero")] private Unit _mainHero;

        public Unit MainHero => _mainHero;

        private async void Start()
        {
            if (_mainHero == null)
            {
                var uploader = GetComponentInChildren<BattleEmergencyUploader>();

                var data = await uploader.UploadNeededData();

                _mainHero = data.party[0];
            }
        }
    }
}