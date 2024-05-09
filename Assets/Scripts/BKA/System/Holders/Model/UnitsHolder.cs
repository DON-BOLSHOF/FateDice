using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using Unit = BKA.Units.Unit;

namespace BKA.System.Holders.Model
{
    public class UnitsHolder : IDisposable
    {
        public List<Unit> Units => _units;
        public IObservable<Unit> OnUnitDataUpdated => _onUnitDataUpdated;
        public IObservable<UniRx.Unit> OnUnitHolderUpdated => _onUnitHolderUpdated;
        
        private List<Unit> _units;
        
        private readonly ReactiveCommand<Unit> _onUnitDataUpdated = new();
        private readonly ReactiveCommand _onUnitHolderUpdated = new();
        
        private readonly CompositeDisposable _holderDisposable = new();

        public UnitsHolder(IEnumerable<Unit> units)
        {
            _units = units.ToList();
            
            foreach (var partyCompanion in units)
            {
                partyCompanion.OnUpdatedData.Subscribe(_ => _onUnitDataUpdated?.Execute(partyCompanion))
                    .AddTo(_holderDisposable);
            }
        }

        public void Add(Unit unit)
        {
            _units.Add(unit);

            unit.OnUpdatedData.Subscribe(_ => _onUnitDataUpdated?.Execute(unit)).AddTo(_holderDisposable);

            _onUnitHolderUpdated?.Execute();
        }
        
        public void Remove(Unit unit)
        {
            _units.Remove(unit);
            
            _onUnitHolderUpdated?.Execute();
        }

        public void Dispose()
        {
            _onUnitDataUpdated?.Dispose();
            _holderDisposable?.Dispose();
        }
    }
}