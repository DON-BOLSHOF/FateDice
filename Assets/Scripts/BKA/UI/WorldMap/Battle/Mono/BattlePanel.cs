using System;
using BKA.Units;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace BKA.UI.WorldMap
{
    public class BattlePanel : MonoBehaviour, IBattlePanel
    {
        [SerializeField] private EnemyWidget[] _enemyWidgets;

        [SerializeField] private Transform _view;
        [SerializeField] private Button _startBattleButton;

        public IObservable<UniRx.Unit> OnActivatedBattle => _activateBattle;

        private ReactiveCommand _activateBattle = new();

        private void Start()
        {
            _startBattleButton.OnClickAsObservable().Subscribe(_ => _activateBattle?.Execute()).AddTo(this);
        }

        public void ActivatePanel()
        {
            _view.gameObject.SetActive(true);
        }

        public void DeactivatePanel()
        {
            _view.gameObject.SetActive(false);
        }

        public void SetData(UnitDefinition[] unitDefinitions)
        {
            if (unitDefinitions.Length > _enemyWidgets.Length)
                throw new ApplicationException("Too many enemy instances");

            int index = 0;
            for (; index < unitDefinitions.Length; index++)
            {
                _enemyWidgets[index].SetData(unitDefinitions[index]);
                _enemyWidgets[index].gameObject.SetActive(true);
            }

            for (; index < _enemyWidgets.Length; index++)
            {
                _enemyWidgets[index].gameObject.SetActive(false);
            }
        }
    }
}