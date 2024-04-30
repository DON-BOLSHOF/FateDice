using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BKA.Dices.DiceActions;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using NotImplementedException = System.NotImplementedException;

namespace BKA.UI.WorldMap
{
    public class Sweep : MonoBehaviour
    {
        [SerializeField] private Image[] _actions;

        private DiceActionData[] _diceActions;

        private CancellationTokenSource _sweepSource = new();

        public void UpdateData(DiceActionData[] actions)
        {
            _diceActions = actions;

            for (var i = 0; i < _actions.Length; i++)
            {
                _actions[i].sprite = actions[i].ActionView;
            }
        }

        public void MakeHint(List<DiceActionPair> specializationDiceActionPairs)
        {
            DeactivateHint();
            _sweepSource = new();

            foreach (var specializationDiceActionPair in specializationDiceActionPairs)
            {
                _actions[specializationDiceActionPair.Index].sprite =
                    specializationDiceActionPair.DiceAction.ActionView;
            }

            ActivateHint(specializationDiceActionPairs.Select(pair => pair.Index).ToArray(), _sweepSource.Token).Forget();
        }

        public void DeactivateHint()
        {
            _sweepSource?.Cancel();

            for (var i = 0; i < _actions.Length; i++)
            {
                _actions[i].sprite = _diceActions[i].ActionView;
                _actions[i].color = Color.white;
            }
        }

        private async UniTaskVoid ActivateHint(int[] indexes, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var uniTasks = Enumerable.Select(indexes, t => _actions[t].DOColor(Color.blue, 1).ToUniTask(cancellationToken: token)).ToList();
                await UniTask.WhenAll(uniTasks);
                uniTasks.Clear();
                uniTasks.AddRange(Enumerable.Select(indexes, t => _actions[t].DOColor(Color.cyan, 1).ToUniTask(cancellationToken: token)));
                await UniTask.WhenAll(uniTasks);
            }
        }

        private void OnDestroy()
        {
            _sweepSource?.Cancel();
        }
    }
}