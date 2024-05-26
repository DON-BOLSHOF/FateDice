using NavMeshPlus.Components;
using UnityEngine;

namespace BKA.System.Navigation.Mono
{
    public class NavigationHandlerMonoReference : MonoBehaviour
    {
        [SerializeField] private NavMeshSurface _meshSurface;

        public NavMeshSurface MeshSurface => _meshSurface;
    }
}