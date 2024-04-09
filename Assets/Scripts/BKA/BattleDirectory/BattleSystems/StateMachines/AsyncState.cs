using System.Threading;
using Cysharp.Threading.Tasks;

namespace BKA.BattleDirectory.BattleSystems.StateMachines
{
    public abstract class AsyncState
    {
        public abstract UniTask Execute(CancellationToken machineSourceToken);
        public abstract UniTask Undo(CancellationToken combineSourceToken);
    }
}