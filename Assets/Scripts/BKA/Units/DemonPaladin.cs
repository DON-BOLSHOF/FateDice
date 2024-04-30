using System;
using BKA.Buffs;
using BKA.Dices.DiceActions;
using Cysharp.Threading.Tasks;
using UniRx;

namespace BKA.Units
{
    public class DemonPaladin : Unit
    {
        public sealed override UnitDefinition Definition { get; protected set; }
        public sealed override DiceActionData[] DiceActions { get; protected set; }
        public sealed override Class Class { get; }

        public DemonPaladin(DefinitionPool definitionPool)
        {
            Definition = definitionPool.GetFromPool("DemonPaladin");

            DiceActions = new DiceActionData[Definition.DiceActions.Length];
            Array.Copy(Definition.DiceActions, DiceActions, Definition.DiceActions.Length);

            _health.Value = Definition.Health;
            Class = new Class(new Specialization(Definition.BaseSpecializationDefinition));
            UpdateData();

            Class.OnDecorated?.Subscribe(_ => UpdateData()).AddTo(_unitDisposable);
        }
    }
}