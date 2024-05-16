using System;
using System.Collections.Generic;
using System.Threading;
using BKA.System.Exceptions;
using BKA.UI.WorldMap.Dialog.Interfaces;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;
using ArgumentOutOfRangeException = System.ArgumentOutOfRangeException;

namespace BKA.UI.WorldMap.Dialog
{
    public class DialogPanel : MonoBehaviour, IDialogPanel, IPointerClickHandler
    {
        [SerializeField] private Transform _view;

        [SerializeField] private ActorWidget _leftActor;
        [SerializeField] private ActorWidget _rightActor;

        [SerializeField] private TextMeshProUGUI _actorName;
        [SerializeField] private TextMeshProUGUI _actorPhrase;

        [SerializeField] private float _speed;

        public IObservable<Unit> OnInputNextTurn => _onInputNextTurn;
        public IObservable<Unit> OnCharSpawned => _onCharSpawned;

        private CharacterPhrase _currentPhrase;
        private ReactiveCommand _onInputNextTurn = new();
        private ReactiveCommand _onCharSpawned = new();

        private bool _isEndedPhrase;

        private CancellationTokenSource _panelSource = new();

        public void ActivatePanel()
        {
            ReverseView();

            _view.gameObject.SetActive(true);
        }

        public void DeactivatePanel()
        {
            _view.gameObject.SetActive(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!TrySkipPhrase())
            {
                _onInputNextTurn?.Execute();
            }
        }

        public  void ActivateNewPhrase(CharacterPhrase characterPhrase)
        {
            _currentPhrase = characterPhrase;

            _actorName.text = characterPhrase.ActorName;
            ActivateNewPhraseAnimation(characterPhrase).Forget();
        }

        private void ReverseView()
        {
            _actorPhrase.text = "";
            _actorName.text = "";
            _leftActor.ClearData();
            _rightActor.ClearData();
        }

        private bool TrySkipPhrase()
        {
            if (_isEndedPhrase) return false;

            _panelSource?.Cancel();
            SkipPhrase();

            return true;
        }

        private void SkipPhrase()
        {
            _actorPhrase.text = _currentPhrase.Phrase;

            switch (_currentPhrase.PhraseActorPosition)
            {
                case PhraseActorPosition.Left:
                    _leftActor.transform.localScale = new Vector3(1.25f, 1.25f, 1);
                    _rightActor.transform.localScale = Vector3.one;
                    break;
                case PhraseActorPosition.Right:
                    _rightActor.transform.localScale = new Vector3(1.25f, 1.25f, 1);
                    _leftActor.transform.localScale = Vector3.one;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private async UniTask ActivateNewPhraseAnimation(CharacterPhrase characterPhrase)
        {
            _panelSource?.Cancel();
            _panelSource = new();

            _isEndedPhrase = false;

            await UniTask.WhenAll(UpdateVoiceActor(characterPhrase, _panelSource.Token),
                UpdateTextPhrase(characterPhrase, _panelSource.Token));

            _isEndedPhrase = true;
        }

        private async UniTask UpdateTextPhrase(CharacterPhrase characterPhrase, CancellationToken token)
        {
            _actorPhrase.text = "";

            var currentChar = 0;

            while (currentChar < characterPhrase.Phrase.Length)
            {
                if (characterPhrase.Phrase[currentChar] == '<')
                {
                    while (currentChar < characterPhrase.Phrase.Length && characterPhrase.Phrase[currentChar] != '>')
                    {
                        _actorPhrase.text += characterPhrase.Phrase[currentChar++];
                    }
                }

                _actorPhrase.text += characterPhrase.Phrase[currentChar++];
                _onCharSpawned?.Execute();
                await UniTask.Delay(TimeSpan.FromSeconds(_speed), cancellationToken: token)
                    .WithPostCancellation(() => _isEndedPhrase = true);
            }
        }

        private async UniTask UpdateVoiceActor(CharacterPhrase characterPhrase, CancellationToken panelSourceToken)
        {
            var tasks = new List<UniTask>();

            switch (characterPhrase.PhraseActorPosition)
            {
                case PhraseActorPosition.Left:
                    _leftActor.SetData(characterPhrase.ActorView);
                    tasks.Add(_leftActor.transform.DOScale(new Vector3(1.25f, 1.25f, 1), _speed * 4)
                        .ToUniTask(cancellationToken: panelSourceToken));
                    tasks.Add(_rightActor.transform.DOScale(Vector3.one, 1)
                        .ToUniTask(cancellationToken: panelSourceToken));
                    break;
                case PhraseActorPosition.Right:
                    _rightActor.SetData(characterPhrase.ActorView);
                    tasks.Add(_rightActor.transform.DOScale(new Vector3(1.25f, 1.25f, 1), _speed * 4)
                        .ToUniTask(cancellationToken: panelSourceToken));
                    tasks.Add(_leftActor.transform.DOScale(Vector3.one, 1)
                        .ToUniTask(cancellationToken: panelSourceToken));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            await UniTask.WhenAll(tasks);
        }
    }
}