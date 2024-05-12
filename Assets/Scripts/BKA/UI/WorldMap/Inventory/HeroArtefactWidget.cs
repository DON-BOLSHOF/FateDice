using System.Linq;
using UniRx;
using Unit = BKA.Units.Unit;

namespace BKA.UI.WorldMap.Inventory
{
    public class HeroArtefactWidget : ItemHolder
    {
        private ReactiveCommand _onUpdatedActions = new();
        private Unit _unit;

        protected override void Awake()
        {
            base.Awake();

            foreach (var inventorySlot in _inventorySlots)
            {
                inventorySlot.OnUpdatedData.Subscribe(_ => UpdateActions()).AddTo(this);
            }
        }

        public void SetHero(Unit unit)
        {
            _unit = unit;
            
            SetArtefacts(unit.Artefacts.ToList());
        }

        private void UpdateActions()
        {
            var artefacts = _inventorySlots.Where(value => value.IsFullFilled)
                .Select(value => value.InventoryItem.Artefact).ToList();

            _unit.SetArtefacts(artefacts);
            _onUpdatedActions?.Execute();
        }
    }
}