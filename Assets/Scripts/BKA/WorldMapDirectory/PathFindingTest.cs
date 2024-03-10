using System;
using UnityEngine;
using UnityEngine.AI;

namespace BKA.WorldMapDirectory
{
    public class PathFindingTest : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _agent;

        [SerializeField] private Animator _animator;

        private static readonly int IsMoving = Animator.StringToHash("IsMoving");

        private bool _hasToDraw;

        private int _currentCorner;

        private void Start()
        {
            _agent.updateRotation = false;
            _agent.updateUpAxis = false;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _agent.SetDestination(Camera.main.ScreenToWorldPoint(Input.mousePosition));

                _hasToDraw = true;

                _currentCorner = _agent.path.corners.Length-1;
                var wayPoint = _agent.path.corners[_currentCorner];

                var temp = (_agent.transform.position - wayPoint).normalized;

                transform.localScale = temp.x < 0 ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);
            }

            if (_agent.hasPath && _hasToDraw)
            {
                _hasToDraw = false;
                Test();
            }

            if (_agent.hasPath && _agent.path.corners.Length>0 && _currentCorner != _agent.path.corners.Length)
            {
                _currentCorner = _agent.path.corners.Length-1;
                
                var wayPoint = _agent.path.corners[_currentCorner];

                var temp = (_agent.transform.position - wayPoint).normalized;

                transform.localScale = temp.x < 0 ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);
            }

            _animator.SetBool(IsMoving, _agent.hasPath);
        }

        private void Test()
        {
            var t = _agent.transform.position;
            t.z = 0;
            foreach (var pathCorner in _agent.path.corners)
            {
                var te = pathCorner;
                te.z = 0;
                
                Debug.DrawLine(t, te, Color.magenta, 10);
                Debug.Log(pathCorner);

                t = te;
            }
        }
    }
}