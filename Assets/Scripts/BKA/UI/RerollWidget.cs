using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace BKA.UI
{
    public class RerollWidget : MonoBehaviour
    {
        [SerializeField] private Button _rerollButton;

        [SerializeField] private TextMeshProUGUI _remainRerolls;
        [SerializeField] private TextMeshProUGUI _totalRerolls;
        
        public ReactiveCommand OnRerolled = new();

        private void Start()
        {
            _rerollButton.OnClickAsObservable().Subscribe(_ => Reroll()).AddTo(this);
        }

        private void Reroll()
        {
            OnRerolled?.Execute();
        }

        public void DynamicInit(IReadOnlyReactiveProperty<int> currentRerolls, int totalRerolls)
        {
            currentRerolls.Subscribe(value => UpdateRerolls(value)).AddTo(this);
            
            _remainRerolls.text = currentRerolls.Value.ToString();
            _totalRerolls.text = totalRerolls.ToString();
        }

        private void UpdateRerolls(int value)
        {
            _remainRerolls.text = value.ToString();
        }
    }
}