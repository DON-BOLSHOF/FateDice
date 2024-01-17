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
            await UniTask.DelayFrame(1);
            
            PrepareData(out var partyBattleBehaviours, out var enemyBattleBehaviours);

            _characterBoarderHandler.DynamicInit(partyBattleBehaviours, enemyBattleBehaviours);
            BindBattleBehavioursWithUIPanels(partyBattleBehaviours, enemyBattleBehaviours, _characterBoarderHandler);
            
            _diceHandler.DynamicInit(partyBattleBehaviours.ToList<IUnitOfBattle>(), enemyBattleBehaviours.ToList<IUnitOfBattle>());

            _fightHandler.DynamicInit(partyBattleBehaviours, enemyBattleBehaviours);
        }

        private void BindBattleBehavioursWithUIPanels(List<UnitBattleBehaviour> partyBattleBehaviours, List<UnitBattleBehaviour> enemyBattleBehaviours, 
            CharacterBoarderHandler characterBoarderHandler)
        {
            for (var i = 0; i < partyBattleBehaviours.Count; i++)
            {
                var panel = characterBoarderHandler.GetCharacterUIPanel(BoarderType.Left,i);

                partyBattleBehaviours[i].BindPanel(panel);
            }

            for (var i = 0; i < enemyBattleBehaviours.Count; i++)
            {
                var panel = characterBoarderHandler.GetCharacterUIPanel(BoarderType.Right,i);
                
                enemyBattleBehaviours[i].BindPanel(panel);
            }
        }

        private void PrepareData(out List<UnitBattleBehaviour> partyBattleBehaviours,
            out List<UnitBattleBehaviour> enemyBattleBehaviours)
        {
            var partyDices = _diceFactory.UploadNewDices(_party.Length, _diceHandler.transform,
                new Vector3(Random.Range(-3, 3), 8, Random.Range(-3, 3)));
            var enemyDices = _diceFactory.UploadNewDices(_enemy.Length, _diceHandler.transform,
                new Vector3(Random.Range(-3, 3), 8, Random.Range(-3, 3)));

            partyBattleBehaviours = new List<UnitBattleBehaviour>();

            for (int i = 0; i < _party.Length; i++)
            {
                var battleBehaviour = new UnitBattleBehaviour(_party[i], partyDices[i]);

                partyDices[i].UpdateActions(_party[i].Definition.DiceActions);

                partyBattleBehaviours.Add(battleBehaviour);
            }

            enemyBattleBehaviours = new List<UnitBattleBehaviour>();

            for (int i = 0; i < _enemy.Length; i++)
            {
                var battleBehaviour = new UnitBattleBehaviour(_enemy[i], enemyDices[i]);

                enemyDices[i].UpdateActions(_enemy[i].Definition.DiceActions);

                enemyBattleBehaviours.Add(battleBehaviour);
            }
        }
    }
}