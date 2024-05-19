using System.Collections.Generic;
using System.Linq;
using BKA.Buffs;
using BKA.System;
using BKA.Units;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Unit = BKA.Units.Unit;

namespace BKA.UI.MainMenu
{
    public class ChoosePanel : MonoBehaviour
    {
        [SerializeField] private Button _startButton;

        [SerializeField] private HeroPanel[] _heroPanels;

        [SerializeField] private TextMeshProUGUI _descriptionText;

        [Inject] private LevelManager _levelManager;
        [Inject] private UnitFactory _unitFactory;

        private HeroPanel _selectedPanel;

        private void Start()
        {
            foreach (var heroPanel in _heroPanels)
            {
                heroPanel.OnHeroPanelClicked.Subscribe(_ => ChangeSelectedHero(heroPanel)).AddTo(this);
            }

            ChangeSelectedHero(_heroPanels[0]);

            _startButton.OnClickAsObservable().Subscribe(_ => StartGame()).AddTo(this);
        }

        private void ChangeSelectedHero(HeroPanel heroPanel)
        {
            foreach (var panel in _heroPanels.Where(value => !value.Equals(heroPanel)))
            {
                panel.UnSelectPanel();
            }

            heroPanel.SelectPanel();
            _descriptionText.text = heroPanel.HeroDefinition.UnitDescription;

            _selectedPanel = heroPanel;
        }

        private void StartGame()
        {
            _levelManager.LoadLevel("WorldMap",
                (container) =>
                {
                    var mainHero = _unitFactory.UploadUnit(_selectedPanel.HeroDefinition);

                    container.Bind<Unit>().WithId("MainHero").FromInstance(mainHero).AsSingle();

                    var heroPack = new List<Unit> { mainHero };
                    heroPack.AddRange(_selectedPanel.HeroBaseCompanionsDefinitions.Select(definition =>
                            _unitFactory.UploadUnit(definition)));

                    foreach (var unit in heroPack)
                    {
                        container.Bind<Unit>().FromInstance(unit).AsCached();
                    } 
                    
                    foreach (var selectedPanelHeroArtefact in _selectedPanel.HeroArtefacts)
                    {
                        container.Bind<Artefact>().FromInstance(selectedPanelHeroArtefact).AsCached();
                    }
                });
        }
    }
}