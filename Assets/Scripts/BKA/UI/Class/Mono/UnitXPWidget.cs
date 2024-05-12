using System.Threading;
using BKA.Units;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BKA.UI
{
    public class UnitXPWidget : MonoBehaviour
    {
        [SerializeField] private Image _heroIcon;
        [SerializeField] private TextMeshProUGUI _heroName;
        [SerializeField] private Slider _slider;

        [SerializeField] private float _timeToSlide;

        public void DynamicInit(Unit unit)
        {
            _heroIcon.sprite = unit.Definition.UnitIcon;
            _heroName.text = unit.Definition.ID;
        }

        public async UniTask UpdateXP(float percentageFrom, float percentageTo, CancellationToken panelSourceToken)
        {
            var percentage = percentageFrom;
            
            await DOTween.To(() => percentage, x => percentage = x, percentageTo, 
                _timeToSlide).OnUpdate(() =>
            {
                _slider.value = percentage;
            }).ToUniTask(cancellationToken: panelSourceToken);
            
            _slider.value = percentageTo;
        }
    }
}