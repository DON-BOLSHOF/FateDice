using System;
using BKA.UI;
using BKA.Units;
using UnityEngine;
using Zenject;

namespace BKA
{
    public class BattleEntryPoint : MonoBehaviour
    {
        [SerializeField] private CharacterBoarderHandler _characterBoarderHandler;
        [SerializeField] private FightHandler _fightHandler;

        [InjectOptional(Id = "Teammates")] private Unit[] _teammates;
        [InjectOptional(Id = "Enemies")] private Unit[] _enemy;
        
        private void Start()
        {
            _characterBoarderHandler.DynamicInit(_teammates, _enemy);
            _fightHandler.DynamicInit(_teammates, _enemy);
        }
    }
}