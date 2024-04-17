using Sirenix.OdinInspector;
using UnityEngine;

namespace Zenject
{
    public abstract class ActivableInstaller : ScriptableObjectInstaller, IActivableInstaller
    {
        public bool IsActive => _isActive;

        [SerializeField, PropertyOrder(-9999), GUIColor("_editorIsActiveColor")]
        protected bool _isActive = true;

#if UNITY_EDITOR
        private Color _editorIsActiveColor => _isActive ? Color.green : Color.red;
#endif
    }

}