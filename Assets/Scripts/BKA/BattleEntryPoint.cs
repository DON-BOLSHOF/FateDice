using BKA.BootsTraps;
using BKA.System.ExtraDirectory;
using BKA.UI;
using BKA.Units;
using UnityEngine;
using Zenject;

namespace BKA
{
    public class BattleEntryPoint : MonoBehaviour
    {
        [SerializeField] private CharacterBoarderHandler _characterBoarderHandler;
        [SerializeField] private FightHandler _fightHandler;

        [InjectOptional(Id = "Teammates")] private Unit[] _teammates;
        [InjectOptional(Id = "Enemies")] private Unit[] _enemy;

        [Inject] private BootsTrapStateObserver _bootsTrapStateObserver;

        private async void Start()
        {
#if UNITY_EDITOR
            if (_bootsTrapStateObserver.BootsTrapState.Value != BootsTrapState.Loaded)
            {
                var uploader = GetComponentInChildren<BattleEmergencyUploader>();
                
                var data= await uploader.UploadNeededData();

                _teammates = data.teammates;
                _enemy = data.enemy;
            }
#else
            if (_bootsTrapStateObserver.BootsTrapState.Value != BootsTrapState.Loaded)
            {
                throw new BootsTrapException();
            }
#endif
            _characterBoarderHandler.DynamicInit(_teammates, _enemy);
            _fightHandler.DynamicInit(_teammates, _enemy);
        }  
    }
}