using System.Collections.Generic;
using System.Threading;
using BKA.Units;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace BKA.UI.WorldMap.Class
{
    public class CharacteristicPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _strengthText;
        [SerializeField] private TextMeshProUGUI _agilityText;
        [SerializeField] private TextMeshProUGUI _intelligentText;

        private int _strengthValue;
        private int _agilityValue;
        private int _intelligentValue;

        private CancellationTokenSource _panelSource = new();

        public void UpdateData(Characteristics classCharacteristics)
        {
            _strengthValue = classCharacteristics.Strength;
            _agilityValue = classCharacteristics.Agility;
            _intelligentValue = classCharacteristics.Intelligent;

            UpdateLocalData();
        }

        public void MakeHint(Characteristics specializationCharacteristics)
        {
            DeactivateHint();
            _panelSource = new();

            ActivateHint(specializationCharacteristics, _panelSource.Token).Forget();
        }

        public void DeactivateHint()
        {
            _panelSource?.Cancel();

            UpdateLocalData();

            _strengthText.color = Color.white;
            _agilityText.color = Color.white;
            _intelligentText.color = Color.white;

            _strengthText.transform.localScale = new Vector3(1, 1, 1);
            _agilityText.transform.localScale = new Vector3(1, 1, 1);
            _intelligentText.transform.localScale = new Vector3(1, 1, 1);
        }

        private async UniTaskVoid ActivateHint(Characteristics characteristics, CancellationToken token)
        {
            var localStrength = characteristics.Strength + _strengthValue;
            var localAgility = characteristics.Agility + _agilityValue;
            var localIntelligent = characteristics.Intelligent + _intelligentValue;

            if (localStrength == _strengthValue && localAgility == _agilityValue &&
                localIntelligent == _intelligentValue)
                return;

            _strengthText.text = localStrength.ToString();
            _agilityText.text = localAgility.ToString();
            _intelligentText.text = localIntelligent.ToString();

            UpdateLocalColor(_strengthText, localStrength, _strengthValue);
            UpdateLocalColor(_agilityText, localAgility, _agilityValue);
            UpdateLocalColor(_intelligentText, localIntelligent, _intelligentValue);

            while (!token.IsCancellationRequested)
            {
                await UpScaleChangedData(localStrength, localAgility, localIntelligent, token);
                await DownScaleChangedData(localStrength, localAgility, localIntelligent, token);
            }
        }

        private async UniTask UpScaleChangedData(int localStrength, int localAgility, int localIntelligent, CancellationToken token)//Вывести в отдельный компонент
        {
            List<UniTask> tasks = new();
            if (localStrength != _strengthValue)
                tasks.Add(_strengthText.transform.DOScale(new Vector3(1.25f, 1.25f, 1.25f), 1).ToUniTask(cancellationToken: token));
            if (localAgility != _agilityValue)
                tasks.Add(_agilityText.transform.DOScale(new Vector3(1.25f, 1.25f, 1.25f), 1).ToUniTask(cancellationToken: token));
            if(localIntelligent != _intelligentValue)
                tasks.Add(_intelligentText.transform.DOScale(new Vector3(1.25f, 1.25f, 1.25f), 1).ToUniTask(cancellationToken: token));

            await UniTask.WhenAll(tasks);
        }

        private async UniTask DownScaleChangedData(int localStrength, int localAgility, int localIntelligent, CancellationToken token)
        {
            List<UniTask> tasks = new();
            if (localStrength != _strengthValue)
                tasks.Add(_strengthText.transform.DOScale(new Vector3(1, 1f, 1f), 1).ToUniTask(cancellationToken: token));
            if (localAgility != _agilityValue)
                tasks.Add(_agilityText.transform.DOScale(new Vector3(1f, 1f, 1f), 1).ToUniTask(cancellationToken: token));
            if(localIntelligent != _intelligentValue)
                tasks.Add(_intelligentText.transform.DOScale(new Vector3(1f, 1f, 1f), 1).ToUniTask(cancellationToken: token));
            await UniTask.WhenAll(tasks);
        }

        private void UpdateLocalColor(TextMeshProUGUI text, int newValue, int oldValue)
        {
            if (newValue > oldValue)
                text.color = Color.green;
            else if (newValue < oldValue)
                text.color = Color.red;
            else
                text.color = Color.white;
        }

        private void UpdateLocalData()
        {
            _strengthText.text = _strengthValue.ToString();
            _agilityText.text = _agilityValue.ToString();
            _intelligentText.text = _intelligentValue.ToString();
        }

        private void OnDestroy()
        {
            _panelSource?.Cancel();
        }
    }
}