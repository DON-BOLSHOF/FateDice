using System;
using System.Collections.Generic;
using BKA.Units;
using UnityEngine;

namespace BKA.UI
{
    public class CharacterBoarderHandler : MonoBehaviour
    {
       [SerializeField] private CharacterBoarder _leftBoarder;
       [SerializeField] private CharacterBoarder _rightBoarder;

       public void DynamicInit(List<UnitBattleBehaviour> teammates, List<UnitBattleBehaviour> enemy)
       {
           _leftBoarder.DynamicInit(teammates);
           _rightBoarder.DynamicInit(enemy);
       }

       public CharacterPanel GetCharacterUIPanel(BoarderType boarderType, int index)
       {
           switch (boarderType)
           {
               case BoarderType.Left:
                   return _leftBoarder.GetPanel(index);
                   break;
               case BoarderType.Right:
                   return _rightBoarder.GetPanel(index);
                   break;
               default:
                   throw new ArgumentOutOfRangeException(nameof(boarderType), boarderType, null);
           }
       }
    }

    public enum BoarderType
    {
        Left,
        Right
    }
}