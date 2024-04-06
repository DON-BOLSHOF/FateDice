using BKA.BattleDirectory.BattleSystems;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BKA.UI
{
    [RequireComponent(typeof(Button))]
    public class TurnWidget : MonoBehaviour, ITurnSystemVisitor
    {
        [Inject] private TurnSystem _turnSystem;

        private Button _turnButton;

        private void Start()
        {
            _turnButton = GetComponent<Button>();
            
            _turnButton.OnClickAsObservable().Subscribe(_ => Accept(_turnSystem)).AddTo(this);
        }

        public void Accept(TurnSystem turnSystem)
        {
            turnSystem.Visit(this);
        }
    }
}