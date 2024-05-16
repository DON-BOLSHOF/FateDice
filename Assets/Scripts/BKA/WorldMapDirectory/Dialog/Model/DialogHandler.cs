﻿using System;
using System.Collections.Generic;
using System.Linq;
using BKA.System;
using BKA.UI.WorldMap.Dialog.Interfaces;
using BKA.Units;
using BKA.WorldMapDirectory.Dialog.Interfaces;
using BKA.Zenject.Signals;
using UniRx;
using UnityEngine;
using Zenject;
using Unit = UniRx.Unit;

namespace BKA.UI.WorldMap.Dialog
{
    public class DialogHandler : IDialogHandler, IDisposable
    {
        public IObservable<Unit> OnDialogEnded => _onDialogEnded;

        private CharacterPhraseProvider[] _currentPhrases;
        private IDialogPanel _dialogPanel;

        private IEnumerable<IDialogPoint> _dialogPoints;

        private int _currentPhrase;

        private Dictionary<Type, Signal> _dialogSideData = new();

        private UnitDefinition _mainHeroDefinition;
        
        private ReactiveCommand _onDialogEnded = new();
        private CompositeDisposable _handlerDisposable = new();
        
        private SignalBus _signalBus;

        public DialogHandler(IEnumerable<IDialogPoint> dialogPoints, IDialogPanel dialogPanel, GameSession gameSession, SignalBus signalBus,
            AudioClip charSpawnedClip)
        {
            _dialogPanel = dialogPanel;
            _dialogPoints = dialogPoints;

            _mainHeroDefinition = gameSession.MainHero.Definition;
            
            _signalBus = signalBus;

            foreach (var dialogPoint in _dialogPoints)
            {
                dialogPoint.OnActivatedDialog.Subscribe(_ => ActivateDialog(dialogPoint.CharacterPhraseProviders))
                    .AddTo(_handlerDisposable);
            }

            _signalBus.Subscribe<ExtraodinaryDialogActivate>(signal =>
                ForceActivateDialog(signal.CharacterPhraseProviders));

            _dialogPanel.OnInputNextTurn.Subscribe(_ => NextPhrase()).AddTo(_handlerDisposable);
            _dialogPanel.OnCharSpawned
                .Subscribe(_ => _signalBus.Fire(new SFXClipSignal { AudioClip = charSpawnedClip }))
                .AddTo(_handlerDisposable);
        }

        public void ForceActivateDialog(CharacterPhraseProvider[] phraseProviders)
        {
            ActivateDialog(phraseProviders);
        }

        private void ActivateDialog(CharacterPhraseProvider[] characterPhrases)
        {
            _signalBus.Fire(new BlockInputSignal { IsBlocked = true });
            
            foreach (var characterPhrase in characterPhrases.Where(phrase => phrase.PhraseActor == PhraseActor.Hero))
            {
                characterPhrase.DynamicSetHeroPhrase(_mainHeroDefinition); //Потенциально нормально, но замени энум на полноценный класс
            }

            _dialogPanel.ActivatePanel();
            _currentPhrases = characterPhrases;
            _currentPhrase = 0;

            _dialogSideData.Clear();

            NextPhrase();
        }

        private void NextPhrase()
        {
            if (_currentPhrase < _currentPhrases.Length)
            {
                switch (_currentPhrases[_currentPhrase].CharacterPhraseState)
                {
                    case CharacterPhraseState.ArtefactHolder:
                        var artefactInsertion = (ArtefactPhraseInsertion)_currentPhrases[_currentPhrase].GetInsertion();
                        switch (artefactInsertion.ArtefactPhraseInsertionState)
                        {
                            case ArtefactPhraseInsertionState.Give:
                                _dialogSideData.Add(typeof(GiveArtefactSignal), new GiveArtefactSignal { Artefact = artefactInsertion.Artefact });
                                break;
                            case ArtefactPhraseInsertionState.Take:
                                _dialogSideData.Add(typeof(TakeArtefactSignal), new TakeArtefactSignal { Artefact = artefactInsertion.Artefact });
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        break;
                    case CharacterPhraseState.HeroHolder:
                        var heroInsertion = (HeroPhraseInsertion)_currentPhrases[_currentPhrase].GetInsertion();
                        _dialogSideData.Add(typeof(UploadNewHeroSignal),
                            new UploadNewHeroSignal { HeroDefinition = heroInsertion.HeroDefinition });
                        break;
                    case CharacterPhraseState.BattleHolder:
                        var battleIntersection = (BattlePhraseInsertion)_currentPhrases[_currentPhrase].GetInsertion();
                        _dialogSideData.Add(typeof(ExtraordinaryBattleSignal), new ExtraordinaryBattleSignal
                            { Enemies = battleIntersection.UnitDefinitions, Xp = battleIntersection.XP });
                        break;
                }

                _dialogPanel.ActivateNewPhrase(_currentPhrases[_currentPhrase++].CharacterPhrase);
            }
            else
            {
                EndDialog();
            }
        }

        private void EndDialog()
        {
            foreach (var signal in _dialogSideData)
            {
                _signalBus.Fire(Convert.ChangeType(signal.Value, signal.Key));
            }

            _dialogPanel.DeactivatePanel();

            _onDialogEnded?.Execute();

            _signalBus.Fire(new BlockInputSignal { IsBlocked = false });
        }

        public void Dispose()
        {
            _handlerDisposable?.Dispose();
        }
    }
}