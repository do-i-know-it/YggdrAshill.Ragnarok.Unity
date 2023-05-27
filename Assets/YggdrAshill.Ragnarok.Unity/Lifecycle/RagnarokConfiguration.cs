#nullable enable
using System;
using System.Linq;
using UnityEngine;

namespace YggdrAshill.Ragnarok.Unity
{
    internal sealed class RagnarokConfiguration : ScriptableObject
    {
        private static RagnarokConfiguration? instance;
        public static ProjectLifecycle? ProjectLifecycle
        {
            get
            {
                if (instance == null)
                {
                    return null;
                }

                return instance.projectLifecycle;
            }
        }

        [SerializeField]
        [Tooltip("Prefab of root lifecycle for all lifecycle in this application.")]
        private ProjectLifecycle? projectLifecycle;

#if UNITY_EDITOR
        [UnityEditor.MenuItem("Assets/Create/YggdrAshill.Ragnarok/Configuration")]
        private static void CreateAsset()
        {
            var path = UnityEditor.EditorUtility.SaveFilePanelInProject("Save RagnarokConfiguration", "RagnarokConfiguration", "asset", string.Empty);

            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            var configuration = CreateInstance<RagnarokConfiguration>();
            
            UnityEditor.AssetDatabase.CreateAsset(configuration, path);

            var preloadedAssets = UnityEditor.PlayerSettings.GetPreloadedAssets().ToList();
            preloadedAssets.RemoveAll(candidate => candidate is RagnarokConfiguration);
            preloadedAssets.Add(configuration);
            
            UnityEditor.PlayerSettings.SetPreloadedAssets(preloadedAssets.ToArray());
        }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void RuntimeInitialize()
        {
            LoadInstanceFromPreloadAssets();
        }

        private static void LoadInstanceFromPreloadAssets()
        {
            var preloadAsset = UnityEditor.PlayerSettings.GetPreloadedAssets().FirstOrDefault(candidate => candidate is RagnarokConfiguration);
            if (preloadAsset is RagnarokConfiguration configuration)
            {
                configuration.OnEnable();
            }
        }
#endif

        private void OnEnable()
        {
            if (!Application.isPlaying)
            {
                return;
            }
            
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                throw new InvalidOperationException($"{nameof(RagnarokConfiguration)} duplicated.");
            }
        }
    }
}
