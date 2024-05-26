using System;

namespace BKA.Buffs
{
    [Flags]
    public enum BuffStatus
    {
        None = 0,
        Actions = 1,
        Characteristics = 2
    }
}