#nullable enable
using YggdrAshill.Ragnarok.Experimental;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace YggdrAshill.Ragnarok
{
    [DefaultExecutionOrder(LifecycleExecutionOrder.Scene)]
    public sealed class SceneLifecycle : Lifecycle
    {
        // TODO: cache result?
        public static SceneLifecycle? InstanceOf(Scene scene)
        {
            if (!scene.IsValid())
            {
                throw new ArgumentException($"{scene} is invalid.");
            }

            foreach (var rootGameObject in scene.GetRootGameObjects())
            {
                if (rootGameObject.TryGetComponent<SceneLifecycle>(out var lifecycle))
                {
                    return lifecycle;
                }
            }

            return null;
        }
        
        private static Stack<SceneLifecycle> OverriddenLifecycleStack { get; } = new Stack<SceneLifecycle>();
        public static SceneLifecycle? OverriddenLifecycle
        {
            get
            {
                if (OverriddenLifecycleStack.Count == 0)
                {
                    return null;
                }

                return OverriddenLifecycleStack.Peek();
            }
        }

        public readonly struct OverrideParentLifecycleScope :
            IDisposable
        {
            private readonly bool isInitialized;

            internal OverrideParentLifecycleScope(SceneLifecycle lifecycle)
            {
                OverriddenLifecycleStack.Push(lifecycle);
                isInitialized = true;
            }

            public void Dispose()
            {
                if (!isInitialized)
                {
                    throw new Exception($"{nameof(OverrideParentLifecycleScope)}");
                }

                OverriddenLifecycleStack.Pop();
            }
        }

        public OverrideParentLifecycleScope OverrideParentLifecycle()
        {
            return new OverrideParentLifecycleScope(this);
        }

        [SerializeField] private bool runAutomatically = true;
        protected override bool RunAutomatically => runAutomatically;
        
        protected override IContext GetCurrentContext()
        {
            var sceneLifecycle = OverriddenLifecycle;
            if (sceneLifecycle != null)
            {
                return sceneLifecycle.CreateChildContext();
            }

            var projectLifecycle = ProjectLifecycle.Instance;
            if (projectLifecycle != null)
            {
                return projectLifecycle.CreateChildContext();
            }

            return new UnityDependencyContext();
        }

        [SerializeField] private ScriptableInstallation[] scriptableInstallationList = Array.Empty<ScriptableInstallation>();
        private IEnumerable<IInstallation> ScriptableInstallationList => scriptableInstallationList;
        
        [SerializeField] private MonoInstallation[] monoInstallationList = Array.Empty<MonoInstallation>();
        private IEnumerable<IInstallation> MonoInstallationList => monoInstallationList;

        [SerializeField] private ScriptableEntryPoint[] scriptableEntryPointList = Array.Empty<ScriptableEntryPoint>();
        [SerializeField] private MonoEntryPoint[] monoEntryPointList = Array.Empty<MonoEntryPoint>();
        protected override IEnumerable<IInstallation> GetInstallationList()
        {
            var scriptableEntryPointInstallationList 
                = scriptableEntryPointList.Select(entryPoint => entryPoint.Installation);
            var monoEntryPointInstallationList
                = monoEntryPointList.Select(entryPoint => entryPoint.Installation);

            return ScriptableInstallationList.Concat(MonoInstallationList)
                .Concat(scriptableEntryPointInstallationList).Concat(monoEntryPointInstallationList);
        }

        protected override void Awake()
        {
            if (transform.parent != null)
            {
                DestroyImmediate(transform.root.gameObject);
                return;
            }

            var instance = InstanceOf(gameObject.scene);

            if (instance != this)
            {
                DestroyImmediate(gameObject);
                return;
            }

            base.Awake();
        }
    }
}
