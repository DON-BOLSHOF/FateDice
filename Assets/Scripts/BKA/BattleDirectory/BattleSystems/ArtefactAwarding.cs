using System;
using BKA.Buffs;
using Zenject;

namespace BKA.BattleDirectory.BattleSystems
{
    public class ArtefactAwarding : IArtefactAwarding
    {
        public Artefact[] Artefacts { get; }
        
        public ArtefactAwarding([InjectOptional] Artefact[] artefacts)
        {
            Artefacts = artefacts ?? Array.Empty<Artefact>();
        }
    }
}