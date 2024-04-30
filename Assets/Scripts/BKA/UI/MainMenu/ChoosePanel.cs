using System.Linq;
using BKA.Buffs;
using BKA.System;
using BKA.Units;
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

            _startButton.OnClickAsObservable().Subscribe(_ => StartBattle()).AddTo(this);
        }

        private void ChangeSelectedHero(HeroPanel heroPanel)
        {
            foreach (var panel in _heroPanels.Where(value => !value.Equals(heroPanel)))
            {
                panel.UnSelectPanel();
            }

            heroPanel.SelectPanel();

            _selectedPanel = heroPanel;
        }

        private void StartBattle()
        {
            _levelManager.LoadLevel("WorldMap",
                (container) =>
                {
                    container.Bind<Unit>().FromInstance(_unitFactory.UploadUnit(_selectedPanel.HeroDefinition))
                        .AsCached();
                    container.Bind<Unit>().FromInstance(_unitFactory.UploadUnit(_heroPanels[0].HeroDefinition))
                        .AsCached();
                    
                    foreach (var selectedPanelHeroArtefact in _selectedPanel.HeroArtefacts)
                    {
                        container.Bind<Artefact>().FromInstance(selectedPanelHeroArtefact).AsCached();
                    }
                });
        }
    }
}