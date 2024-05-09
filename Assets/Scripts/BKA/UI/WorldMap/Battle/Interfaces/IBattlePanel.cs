using System;
using BKA.Units;
using Unit = UniRx.Unit;

namespace BKA.UI.WorldMap
{
    public interface IBattlePanel
    {
        IObservable<Unit> OnActivatedBattle { get; }

        void SetData(UnitDefinition[] unitDefinitions);
        
        void ActivatePanel();
        void DeactivatePanel();
    }
}