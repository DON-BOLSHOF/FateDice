using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace BKA.UI
{
    [RequireComponent(typeof(Button))]
    public class RerollWidget : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _remainRerolls;
        [SerializeField] private TextMeshProUGUI _totalRerolls;
        
        public ReactiveCommand OnRerolled = new();

        private Button _rerollButton;

        public void DynamicInit(IReadOnlyReactiveProperty<int> currentRerolls, int totalRerolls)
        {
            currentRerolls.Subscribe(UpdateRerolls).AddTo(this);
            
            _remainRerolls.text = currentRerolls.Value.ToString();
            _totalRerolls.text = totalRerolls.ToString();
        }

        private void Start()
        {
            _rerollButton = GetComponent<Button>();
            
            _rerollButton.OnClickAsObservable().Subscribe(_ => Reroll()).AddTo(this);
        }

        private void Reroll()
        {
            OnRerolled?.Execute();
        }

        private void UpdateRerolls(int value)
        {
            _remainRerolls.text = value.ToString();
        }
    }
}