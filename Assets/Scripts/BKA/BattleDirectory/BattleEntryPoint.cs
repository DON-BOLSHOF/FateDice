using System;
using System.Collections.Generic;
using System.Linq;
using BKA.BattleDirectory.BattleHandlers;
using BKA.BattleDirectory.ReadinessObserver;
using BKA.BootsTraps;
using BKA.Dices;
using BKA.System.Exceptions;
using BKA.System.ExtraDirectory;
using BKA.UI;
using BKA.Units;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;
using Unit = BKA.Units.Unit;

namespace BKA.BattleDirectory
{
    public class BattleEntryPoint : MonoBehaviour, IReadinessObservable
    {
        [SerializeField] private CharacterBoarderHandler _characterBoarderHandler;
        [SerializeField] private FightHandler _fightHandler;
        [SerializeField] private DiceHandler _diceHandler;

        [InjectOptional(Id = "Party")] private Unit[] _party;
        [InjectOptional(Id = "Enemies")] private Unit[] _enemy;

        [Inject] private BootsTrapStateObserver _bootsTrapStateObserver;

        [Inject] private DiceFactory _diceFactory;

        [Inject] private UnitBattleBehaviourUploader _behaviourUploader;

        private ReactiveProperty<bool> _isReady = new(false);

        public ReadOnlyReactiveProperty<bool> IsReadyAbsolutely => _isReady.ToReadOnlyReactiveProperty();

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
            await UniTask.DelayFrame(10);
            PrepareData(out var partyBattleBehaviours, out var enemyBattleBehaviours);
            _isReady.Value = true;
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