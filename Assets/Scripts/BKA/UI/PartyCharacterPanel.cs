using UniRx;
using UnityEngine;

namespace BKA.UI
{
    public class PartyCharacterPanel : CharacterPanel
    {
        [SerializeField] private CharacterStateWidget _characterStateWidget;
        
        [SerializeField] private Transform _view;
        
        private Vector3 _viewBasePosition;

        protected void Start()
        {
            _viewBasePosition = _view.localPosition;
            _characterStateWidget.OnClicked.Subscribe(_ =>
            {
                _unitBattleBehaviour.DiceObject.UnSelectDice();
            }).AddTo(this);
        }
        
        public void SetActing()
        {
            _view.localPosition += new Vector3(50f, 0, 0);
        }

        public void SetUnActing()
        {
            _view.localPosition = _viewBasePosition;
        }
    }
}