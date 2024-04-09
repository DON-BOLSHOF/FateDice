using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace BKA.BattleDirectory.BattleSystems.StateMachines
{
    public class StateMachine
    {
        private List<AsyncState> _statesSequence;

        private int _currentStateIndex;

        private bool _isActing;

        private CancellationTokenSource _machineSource = new();

        public StateMachine(IEnumerable<AsyncState> states)
        {
            _statesSequence = states.ToList();
        }

        public async UniTask StartNewSequence(CancellationToken machineSourceToken)
        {
            _isActing = true;

            _machineSource?.Cancel();
            _machineSource = new();
            
            var combineSource = CancellationTokenSource.CreateLinkedTokenSource(machineSourceToken,_machineSource.Token);
            
            _currentStateIndex = 0;
            await ActivateSequence(combineSource.Token);
            
            _isActing = false;
        }

        public async UniTask StepBack(CancellationToken machineSourceToken)
        {
            _machineSource?.Cancel();
            _machineSource = new();

            var combineSource = CancellationTokenSource.CreateLinkedTokenSource(machineSourceToken,_machineSource.Token);
            
            await _statesSequence[_currentStateIndex].Undo(combineSource.Token);

            _currentStateIndex--;
        }

        public async UniTask ContinueSequence(CancellationToken machineSourceToken)
        {
            _machineSource?.Cancel();
            _machineSource = new();
            
            var combineSource = CancellationTokenSource.CreateLinkedTokenSource(machineSourceToken,_machineSource.Token);

            await ActivateSequence(combineSource.Token);
        }
        
        private async UniTask ActivateSequence(CancellationToken machineSourceToken)
        {
            while (_currentStateIndex < _statesSequence.Count)
            {
                await _statesSequence[_currentStateIndex].Execute(machineSourceToken);
                _currentStateIndex++;
            }
        }
    }
}