using UniRx;
using UnityEngine;

namespace BKA.UI
{
    public class PartyCharacterPanel : CharacterPanel
    {
        [SerializeField] private CharacterStateWidget _characterStateWidget;
        
        [SerializeField] private Transform _view;
        
        private static readonly int Acting = Animator.StringToHash("Acting");

        protected void Start()
        {
            _characterStateWidget.OnClicked.Subscribe(_ =>
            {
                _unitBattleBehaviour.DiceObject.UnSelectDice();
            }).AddTo(this);
        }
        
        public void SetActing()
        {
            _characterPanelAnimator.SetBool(Acting, true);
        }

        public void SetUnActing()
        {
            _characterPanelAnimator.SetBool(Acting, false);
        }
    }
}