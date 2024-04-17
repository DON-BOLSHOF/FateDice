﻿using System;
using BKA.Units;
using BKA.WorldMapDirectory.Artefacts;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Unit = UniRx.Unit;

namespace BKA.UI.MainMenu
{
    public class HeroPanel : MonoBehaviour
    {
        [SerializeField] private UnitDefinition _heroDefinition;
        [SerializeField] private Artefact[] _heroArtefacts;
        
        [SerializeField] private Button _panelButton;
        [SerializeField] private Image _pedestal;

        public UnitDefinition HeroDefinition => _heroDefinition;
        public Artefact[] HeroArtefacts => _heroArtefacts;
        public IObservable<Unit> OnHeroPanelClicked => _onHeroPanelClicked;

        private ReactiveCommand _onHeroPanelClicked = new();

        public void SelectPanel()
        {
            _pedestal.color = Color.cyan;
        }

        public void UnSelectPanel()
        {
            _pedestal.color = Color.white;
        }

        private void Start()
        {
            _panelButton.OnClickAsObservable().Subscribe(_ => _onHeroPanelClicked?.Execute()).AddTo(this);
        }
    }
}