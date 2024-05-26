using System;
using BKA.System.Navigation.Mono;
using BKA.System.UploadData;
using NavMeshPlus.Components;
using UniRx;

namespace BKA.System.Navigation.Model
{
    public class NavigationHandler : IDisposable
    {
        private NavMeshSurface _meshSurface;

        private CompositeDisposable _handlerDisposable = new();
        
        public NavigationHandler(NavigationHandlerMonoReference monoReference, NavigationSynchronizer navigationSynchronizer)
        {
            _meshSurface = monoReference.MeshSurface;
            _meshSurface.enabled = false;

            navigationSynchronizer.IsSynchrolized.Where(value => value).Subscribe(_ => 
                _meshSurface.enabled = true).AddTo(_handlerDisposable);
        }

        public void Dispose()
        {
            _handlerDisposable?.Dispose();
        }
    }
}