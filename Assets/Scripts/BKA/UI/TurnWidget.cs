using BKA.BattleDirectory;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using NotImplementedException = System.NotImplementedException;

namespace BKA.UI
{
    public class TurnWidget : MonoBehaviour, ITurnSystemVisitor
    {
        [Inject] private TurnSystem _turnSystem;

        [SerializeField] private Button _turnButton;

        private void Start()
        {
            _turnButton.OnClickAsObservable().Subscribe(_ => Accept(_turnSystem)).AddTo(this);
        }

        public void Accept(TurnSystem turnSystem)
        {
            turnSystem.Visit(this);
        }
    }
}