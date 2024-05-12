using System;
using BKA.WorldMapDirectory.Systems;
using BKA.Zenject.Signals;
using UniRx;
using UnityEngine;
using Zenject;

namespace BKA.WorldMapDirectory.Quest
{
    public class BattleQuestElement : QuestElement
    {
        [SerializeField] private Transform _hintObject;
        [SerializeField] private string _questHint;
        [SerializeField] private BattlePoint _battlePoint;

        [Inject] private SignalBus _signalBus;

        public override string QuestElementHint => _questHint;
        public IObservable<Unit> OnBattleActivated => _onBattleActivated;

        private ReactiveCommand _onBattleActivated = new();

        private void Start()
        {
            _battlePoint.gameObject.SetActive(false);
            _battlePoint.OnBattleStart.Subscribe(valueTuple =>
            {
                _onBattleActivated?.Execute();
                _signalBus.Fire(new ExtraordinaryBattleSignal { Enemies = valueTuple.Item1, Xp = valueTuple.Item2 });
            }).AddTo(this);
        }

        public override void Activate()
        {
            _battlePoint.gameObject.SetActive(true);
            _hintObject.gameObject.SetActive(true);
        }

        public void CastEndBattle()
        {
            OnBattleEnded();
        }

        public override int GetHashCode()
        {
            return QuestElementHint.GetHashCode();
        }

        private void OnBattleEnded()
        {
            _onElementCompleted?.Execute();
            _hintObject.gameObject.SetActive(false);
        }
    }
}