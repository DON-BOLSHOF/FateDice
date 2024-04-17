using System;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Zenject
{
    [CreateAssetMenu(menuName = "Installers/InstallersGroup", fileName = "InstallersGroup")]
    public class InstallersGroup : ActivableInstaller
    {
        [SerializeField, Tooltip("           ")]
        protected bool _autoReferences = true;

        [SerializeField, ShowIf("_autoReferences")]
        protected bool _subfolderSearch = false;

        [SerializeField, HideIf("_autoReferences")]
        protected ScriptableObjectInstallerBase[] _scriptableObjectInstallers;

        [SerializeField, ShowIf("@_autoReferences && UnityEngine.Application.isPlaying"), ReadOnly]
        protected ScriptableObjectInstallerBase[] _installersAutoReferences;

        [SerializeField, ShowIf("_autoReferences"), FolderPath,
         InfoBox("/<AssetDirectory>/<AssetName>/,")]
        protected string[] _additionalInstallersSearchFolders;


        public override void InstallBindings()
        {
            InstallGroup(Container);
        }

        protected virtual void InstallGroup(DiContainer container)
        {
            foreach (var installer in _autoReferences ? _installersAutoReferences : _scriptableObjectInstallers)
            {
                if (installer is IActivableInstaller { IsActive: false })
                {
                    continue;
                }

                container.Inject(installer);
                installer.InstallBindings();
            }
        }

#if UNITY_EDITOR
        public static IEnumerable<InstallersGroup> EditorGetAll()
        {
            foreach (var guid in UnityEditor.AssetDatabase.FindAssets("t:" + nameof(InstallersGroup)))
            {
                var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);

                yield return UnityEditor.AssetDatabase.LoadAssetAtPath<InstallersGroup>(path);
            }
        }

        public virtual void EditorUpdateReferences()
        {
            if (_autoReferences)
            {
                var assetFile = new FileInfo(UnityEditor.AssetDatabase.GetAssetPath(this));
                var subInstallersDirectory = Path.Combine(assetFile.Directory.FullName,
                    Path.GetFileNameWithoutExtension(assetFile.Name));
                var installers = new List<ScriptableObjectInstallerBase>();

                if (Directory.Exists(subInstallersDirectory))
                {
                    var subInstallersProjectDirectory =
                        subInstallersDirectory.Substring(Application.dataPath.Length - 6);

                    SearchAssets(installers, subInstallersProjectDirectory);
                }

                if (_additionalInstallersSearchFolders != null)
                {
                    foreach (var additionalInstallersSearchFolder in _additionalInstallersSearchFolders)
                    {
                        if (!string.IsNullOrEmpty(additionalInstallersSearchFolder))
                        {
                            SearchAssets(installers, additionalInstallersSearchFolder);
                        }
                    }
                }

                _installersAutoReferences = installers.ToArray();
                UnityEditor.EditorUtility.SetDirty(this);
            }
        }

        public virtual void EditorClearReferences()
        {
            if (_installersAutoReferences != null && _installersAutoReferences.Length > 0)
            {
                _installersAutoReferences = Array.Empty<ScriptableObjectInstallerBase>();
                UnityEditor.EditorUtility.SetDirty(this);
            }
        }

        protected virtual void SearchAssets(List<ScriptableObjectInstallerBase> installers, string searchPath)
        {
            searchPath = searchPath.Replace("/", "\\");

            foreach (var guid in UnityEditor.AssetDatabase.FindAssets("t:" + nameof(ScriptableObjectInstallerBase),
                         new[] { searchPath }))
            {
                var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid).Replace("/", "\\");

                if (_subfolderSearch || Path.GetDirectoryName(path) == searchPath)
                {
                    var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<ScriptableObjectInstallerBase>(path);

                    if (asset != this && !installers.Contains(asset) && EditorInstallerIsValid(asset))
                    {
                        installers.Add(asset);
                    }
                }
            }
        }

        protected virtual bool EditorInstallerIsValid(ScriptableObjectInstallerBase installer)
        {
            return true;
        }
#endif
    }
}