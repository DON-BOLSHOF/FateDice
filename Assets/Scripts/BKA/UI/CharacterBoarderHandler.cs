using BKA.Units;
using UnityEngine;

namespace BKA.UI
{
    public class CharacterBoarderHandler : MonoBehaviour
    {
       [SerializeField] private CharacterBoarder _leftBoarder;
       [SerializeField] private CharacterBoarder _rightBoarder;

       public void DynamicInit(UnitDefinition[] teammates, UnitDefinition[] enemy)
       {
           _leftBoarder.DynamicInit(teammates);
           _rightBoarder.DynamicInit(enemy);
       }
    }
}