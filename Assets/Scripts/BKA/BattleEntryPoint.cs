using System.Collections.Generic;
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
        [SerializeField] private DiceHandler _diceHandler;

        [InjectOptional(Id = "Teammates")] private Unit[] _teammates;
        [InjectOptional(Id = "Enemies")] private Unit[] _enemy;

        [Inject] private BootsTrapStateObserver _bootsTrapStateObserver;

        private async void Start()
        {
#if UNITY_EDITOR
            if (_bootsTrapStateObserver.BootsTrapState.Value != BootsTrapState.Loaded)
            {
                var uploader = GetComponentInChildren<BattleEmergencyUploader>();

                var data = await uploader.UploadNeededData();

                _teammates = data.teammates;
                _enemy = data.enemy;
            }
#else
            if (_bootsTrapStateObserver.BootsTrapState.Value != BootsTrapState.Loaded)
            {
                throw new BootsTrapException();
            }
#endif
            var dices = _diceHandler.UploadNewDices(_teammates.Length + _enemy.Length);

            var teammateBattleBehaviours = new List<UnitBattleBehaviour>();

            for (int i = 0; i < _teammates.Length; i++)
            {
                var battleBehaviour = new UnitBattleBehaviour(_teammates[i], dices[i]);

                dices[i].DynamicInit(_teammates[i].Definition.DiceActions);

                teammateBattleBehaviours.Add(battleBehaviour);
            }
            
            var enemyBattleBehaviours = new List<UnitBattleBehaviour>();

            for (int i = 0; i < _enemy.Length; i++)
            {
                var battleBehaviour = new UnitBattleBehaviour(_enemy[i], dices[i + _teammates.Length]);

                dices[i + _teammates.Length].DynamicInit(_enemy[i].Definition.DiceActions);

                enemyBattleBehaviours.Add(battleBehaviour);
            }

            _characterBoarderHandler.DynamicInit(_teammates, _enemy);
            _fightHandler.DynamicInit(_teammates, _enemy);
        }  
    }
}