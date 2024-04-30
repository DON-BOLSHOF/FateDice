using System.Collections.Generic;
using BKA.Buffs;
using UnityEngine;

namespace Zenject.Class.Specialization
{
    [CreateAssetMenu(menuName = "Installers/Class/Specialization/SpecializationIdentifierInstaller", fileName = "SpecializationIdentifierInstaller")]
    public class SpecializationIdentifierInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private List<SpecializationSequence> _sequences;
        
        public override void InstallBindings()
        {
            Container.Bind<SpecializationIdentifier>().AsSingle().WithArguments(_sequences);
        }
    }
}