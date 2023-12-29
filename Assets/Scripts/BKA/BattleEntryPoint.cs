using System;
using BKA.Characters;
using BKA.UI;
using UnityEngine;
using Zenject;

namespace BKA
{
    public class BattleEntryPoint : MonoBehaviour
    {
        [SerializeField] private CharacterBoarderHandler _characterBoarderHandler;
        [SerializeField] private FightHandler _fightHandler;

        public BattleEntryPoint([InjectOptional] Character[] teammates,[InjectOptional] Character[] enemy)
        {
            _fightHandler.DynamicInit(teammates, enemy);
        }
    }
}