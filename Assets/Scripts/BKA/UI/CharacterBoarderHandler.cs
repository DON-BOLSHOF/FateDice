using System;
using BKA.System;
using BKA.Units;
using UniRx;
using UnityEngine;
using Zenject;

namespace BKA.UI
{
    public class CharacterBoarderHandler : MonoBehaviour, ISyncSystem
    {
        [SerializeField] private CharacterBoarder _leftBoarder;
        [SerializeField] private CharacterBoarder _rightBoarder;

        [Inject] private UnitBattleBehaviourUploader _behaviourUploader;

        private ReactiveProperty<bool> _isSynchrolized = new();

        public IReadOnlyReactiveProperty<bool> IsSynchrolized => _isSynchrolized;

        private void Start()
        {
            _behaviourUploader.OnUploadedBehaviour.Subscribe(value => BindUI(value.Item1, value.Item2)).AddTo(this);
        }

        private void BindUI(UnitBattleBehaviour unit, UnitSide side)
        {
            switch (side)
            {
                case UnitSide.Party:
                    _leftBoarder.AddNewBehaviour(unit);
                    unit.OnDead.Subscribe(_ =>
                    {
                        _leftBoarder.RemoveBehaviour(unit);
                        _isSynchrolized.Value = false;
                    }).AddTo(this);
                    break;
                case UnitSide.Enemy:
                    _rightBoarder.AddNewBehaviour(unit);
                    unit.OnDead.Subscribe(_ =>
                    {
                        _rightBoarder.RemoveBehaviour(unit);
                        _isSynchrolized.Value = false;
                    }).AddTo(this);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(side), side, null);
            }

            _isSynchrolized.Value = false;
        }

        public (Vector3[] partyBaseUIPositions, Vector3[] enemyBaseUIPosition) GetCurrentUIPositionsInWorldSpace()
        {
            return (_leftBoarder.GetAttributePositionsInWorld(), _rightBoarder.GetAttributePositionsInWorld());
        }
        
        public void Synchronize(BattleSynchronizer synchronizer)
        {
            _isSynchrolized.Value = true;
        }
    }
}