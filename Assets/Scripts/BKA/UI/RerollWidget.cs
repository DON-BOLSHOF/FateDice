using BKA.BattleDirectory;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BKA.UI
{
    public class RerollWidget : MonoBehaviour
    {
        [SerializeField] private Button _rerollButton;

        [SerializeField] private TextMeshProUGUI _remainRerolls;
        [SerializeField] private TextMeshProUGUI _totalRerolls;

        [Inject] private TurnSystem _turnSystem;

        public ReactiveCommand OnRerolled = new();

        [SerializeField] private int _totalRerollsCount = 2; //Вывести в отдельную систему!!!
        private int _remainRerollsCount;

        [SerializeField] private DiceHandler _diceHandler; //Вывести в отдельный модуль

        private void Start()
        {
            _rerollButton.OnClickAsObservable().Subscribe(_ => Reroll()).AddTo(this);

            _turnSystem.TurnState.Subscribe(_ => UpdateRerolls()).AddTo(this);

            _remainRerolls.text = _remainRerollsCount.ToString();
            _totalRerolls.text = _totalRerollsCount.ToString();
        }

        private void Reroll()
        {
            OnRerolled?.Execute();

            //Вывести
            if (_remainRerollsCount > 0)
            {
                _diceHandler.Shake();
                _remainRerollsCount--;
                _remainRerolls.text = _remainRerollsCount.ToString();
            }
        }

        private void UpdateRerolls()
        {
            _remainRerollsCount = _totalRerollsCount;
            _remainRerolls.text = _remainRerollsCount.ToString();
        }
    }
}