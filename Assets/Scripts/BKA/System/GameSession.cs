using System.Collections.Generic;
using System.Linq;
using BKA.Units;
using BKA.WorldMapDirectory.Artefacts;
using Zenject;

namespace BKA.System
{
    public class GameSession
    {
        private readonly List<Unit> _partyCompanions;
        private readonly List<Artefact> _artefacts;

        public List<Unit> Party => _partyCompanions;
        public List<Artefact> Artefacts => _artefacts;
 
        public GameSession(IEnumerable<Unit> partyCompanions, [InjectOptional] IEnumerable<Artefact> artefacts)
        {
            _partyCompanions = partyCompanions.ToList();

            _artefacts = artefacts != null ? artefacts.ToList() : new();
        }
    }
}