using System;
using System.Collections.Generic;
using System.Linq;
using BKA.BattleDirectory.BattleSystems;
using BKA.Units;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BKA.BattleDirectory
{
    public class Boarder : MonoBehaviour
    {
        [SerializeField] private Transform _upBoarder,
            _downBoarder,
            _leftBoarder,
            _rightBoarder;

        [SerializeField] private float _offsetOfEdges = 0.5f;

        private Vector3[,] _boardMatrix;

        private void Start()
        {
            var downPos = (_downBoarder.localPosition.z + _offsetOfEdges)+1;
            var upPos = (_upBoarder.localPosition.z - _offsetOfEdges)-1;

            var rightPos = (_rightBoarder.localPosition.x - _offsetOfEdges)-1;
            var leftPos = (_leftBoarder.localPosition.x + _offsetOfEdges)+1;

            _boardMatrix = new Vector3[(int)Math.Abs(downPos - upPos) + 1, (int)Math.Abs(rightPos - leftPos) + 1];

            for (var i = 0; i < _boardMatrix.GetLength(0); i++)
            {
                for (var j = 0; j < _boardMatrix.GetLength(1); j++)
                {
                    _boardMatrix[i, j] = new Vector3(leftPos + j, 5, upPos-i);
                }
            }
        }

        public List<Vector3> GeneratePositionsToMove(int cubeCount)
        {
            var result = new List<Vector3>();
            result.AddRange(Enumerable.Repeat(Vector3.zero, cubeCount));
            
            for (var i = 0; i < cubeCount;)
            {
                var randomX = Random.Range(0, _boardMatrix.GetLength(0));
                var randomZ = Random.Range(0, _boardMatrix.GetLength(1));

                if (result.Find(value => value.Equals(_boardMatrix[randomX, randomZ])) != default) continue;
                
                result[i] = _boardMatrix[randomX, randomZ];
                i++;
            }

            return result;
        }

        public List<Vector3> GenerateProportionalPositionsToMove(int cubeCount, TurnState turnState)
        {
            var result = new List<Vector3>();

            for (int i = _boardMatrix.GetLength(0) / 4;cubeCount>0 && i < _boardMatrix.GetLength(0) - _boardMatrix.GetLength(0)/4;i++)
            {
                var startThreshold = 0;
                var endThreshold = _boardMatrix.GetLength(1);
                
                for (int j =0; cubeCount > 0 && j < i - _boardMatrix.GetLength(1) / 4 + 1 && startThreshold<_boardMatrix.GetLength(1); j++)
                {
                    var randomZ = Random.Range(startThreshold, endThreshold);
                    
                    result.Add(_boardMatrix[i,randomZ]);

                    if (turnState == TurnState.PartyTurn)
                    {
                        endThreshold = randomZ;
                    }
                    else
                    {
                        startThreshold = randomZ + 1;
                    }

                    cubeCount--;
                }
            }

            return result;
        }
    }
}