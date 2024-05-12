using System;
using BKA.Units;

namespace BKA.UI
{
    public interface IUpdateXPPanel
    {
        IObservable<UniRx.Unit> OnCompleted { get; }
        void ActivatePanel(Unit[] units, float[] xpFrom, float[] xpTo);
    }
}