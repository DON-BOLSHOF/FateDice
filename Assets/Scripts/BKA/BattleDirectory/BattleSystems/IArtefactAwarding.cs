﻿using BKA.WorldMapDirectory.Artefacts;

namespace BKA.BattleDirectory.BattleSystems
{
    public interface IArtefactAwarding
    {
        Artefact[] Artefacts { get; }
    }
}