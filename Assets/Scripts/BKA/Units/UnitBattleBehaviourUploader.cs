using BKA.BattleDirectory;
using BKA.Dices;
using BKA.UI;
using UniRx;
using Zenject;

namespace BKA.Units
{
    public class UnitBattleBehaviourUploader
    {
        [Inject] private DicePool _dicePool;
        [Inject] private DiceFactory _diceFactory;
        [Inject] private DiceHandler _diceHandler;

        public ReactiveCommand<(UnitBattleBehaviour, UnitSide)> OnUploadedBehaviour = new();

        public UnitBattleBehaviour UploadNewBattleBehaviour(Unit unit, UnitSide unitSide)
        {
            if (!_dicePool.GetCubeDice(out var diceForBehaviour))
            {
                diceForBehaviour = _diceFactory.CreateCubeDice(_diceHandler.transform);
            }

            var behaviour = new UnitBattleBehaviour(unit, diceForBehaviour);

            var behaviourDisposable = new CompositeDisposable();
            behaviour.OnDead.Subscribe(_ => _dicePool.ReplenishDice(behaviour.DiceObject)).AddTo(behaviourDisposable);
            
            behaviour.OnDead.Subscribe(_ =>
            {
                behaviourDisposable.Dispose();
                behaviourDisposable.Clear();
                behaviourDisposable = null;
            }).AddTo(behaviourDisposable);

            OnUploadedBehaviour?.Execute((behaviour, unitSide));
            
            return behaviour;
        }
    }

    public enum UnitSide
    {
        Party,
        Enemy
    }
}