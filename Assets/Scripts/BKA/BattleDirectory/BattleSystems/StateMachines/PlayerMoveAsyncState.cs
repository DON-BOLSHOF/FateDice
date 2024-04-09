using System.Threading;
using Cysharp.Threading.Tasks;

namespace BKA.BattleDirectory.BattleSystems.StateMachines
{
    public class PlayerMoveAsyncState : AsyncState
    {
        public async override UniTask Execute(CancellationToken machineSourceToken)
        {
            
        }

        public async override UniTask Undo(CancellationToken combineSourceToken)
        {
        }
    }
}