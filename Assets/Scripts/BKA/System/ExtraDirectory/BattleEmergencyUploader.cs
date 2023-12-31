﻿using BKA.Units;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace BKA.System.ExtraDirectory
{
    public class BattleEmergencyUploader : MonoBehaviour
    {
        [Inject] private DefinitionPool _definitionPool;

        public async UniTask<(Unit[] teammates, Unit[] enemy)> UploadNeededData()
        {
            await _definitionPool.UploadBaseDefinitions();

            return (new Unit[] { new DemonPaladin(_definitionPool) }, new Unit[] { new DemonPaladin(_definitionPool) });
        }
    }
}