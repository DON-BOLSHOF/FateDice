using System.Collections.Generic;
using System.Linq;
using BKA.BootsTraps;
using BKA.Dices;
using BKA.System.ExtraDirectory;
using BKA.UI;
using BKA.Units;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace BKA.BattleDirectory
{
    public class BattleEntryPoint : MonoBehaviour
    {
        [SerializeField] private CharacterBoarderHandler _characterBoarderHandler;
        [SerializeField] private FightHandler _fightHandler;
        [SerializeField] private DiceHandler _diceHandler;

        [InjectOptional(Id = "Party")] private Unit[] _party;
        [InjectOptional(Id = "Enemies")] private Unit[] _enemy;

        [Inject] private BootsTrapStateObserver _bootsTrapStateObserver;

        [Inject] private DiceFactory _diceFactory;

        [Inject] private UnitBattleBehaviourUploader _behaviourUploader;

        private async void Start()
        {
#if UNITY_EDITOR
            if (_bootsTrapStateObserver.BootsTrapState.Value != BootsTrapState.Loaded)
            {
                var uploader = GetComponentInChildren<BattleEmergencyUploader>();

                var data = await uploader.UploadNeededData();

                _party = data.party;
                _enemy = data.enemy;
            }
#else
            if (_bootsTrapStateObserver.BootsTrapState.Value != BootsTrapState.Loaded)
            {
                throw new BootsTrapException();
            }
#endif
            PrepareData(out var partyBattleBehaviours, out var enemyBattleBehaviours);
        }

        private void PrepareData(out List<UnitBattleBehaviour> partyBattleBehaviours,
            out List<UnitBattleBehaviour> enemyBattleBehaviours)
        {
            enemyBattleBehaviours = new();

            partyBattleBehaviours = _party.Select(unit => _behaviourUploader.UploadNewBattleBehaviour(unit, UnitSide.Party)).ToList();
            enemyBattleBehaviours = _enemy.Select(unit => _behaviourUploader.UploadNewBattleBehaviour(unit, UnitSide.Enemy)).ToList();
        }
    }
}