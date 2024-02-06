#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    [DefaultExecutionOrder(LifecycleExecutionOrder.Scene)]
    public sealed class SceneLifecycle : Lifecycle
    {
        public static bool FindInstance(Scene scene, out SceneLifecycle lifecycle)
        {
            if (!scene.IsValid())
            {
                throw new ArgumentException($"{scene} is invalid.");
            }

            lifecycle = default!;

            // TODO: object pooling.
            var buffer = new List<GameObject>();

            scene.GetRootGameObjects(buffer);
            
            foreach (var instance in buffer)
            {
                if (instance.TryGetComponent(out lifecycle))
                {
                    return true;
                }
            }

            return false;
        }
        
        private static Stack<SceneLifecycle> OverriddenLifecycleStack { get; } = new();
        public static bool FindOverriddenLifecycle(out SceneLifecycle lifecycle)
        {
            lifecycle = default!;
            
            if (OverriddenLifecycleStack.Count == 0)
            {
                return false;
            }

            lifecycle = OverriddenLifecycleStack.Peek();

            return true;
        }

        public readonly struct OverrideParentLifecycleScope : IDisposable
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
        
        protected override IObjectContext GetCurrentContext()
        {
            if (FindOverriddenLifecycle(out var sceneLifecycle))
            {
                return sceneLifecycle.CreateContext();
            }

            if (ProjectLifecycle.FindInstance(out var projectLifecycle))
            {
                return projectLifecycle.CreateContext();
            }

            return new UnityDependencyContext();
        }

        [SerializeField] private ScriptableInstallation[] scriptableInstallationList = Array.Empty<ScriptableInstallation>();
        private IEnumerable<IInstallation> ScriptableInstallationList => scriptableInstallationList;
        
        [SerializeField] private MonoInstallation[] monoInstallationList = Array.Empty<MonoInstallation>();
        private IEnumerable<IInstallation> MonoInstallationList => monoInstallationList;

        protected override IEnumerable<IInstallation> GetInstallationList()
        {
            return ScriptableInstallationList.Concat(MonoInstallationList);
        }

        protected override void Awake()
        {
            if (transform.parent != null)
            {
                DestroyImmediate(transform.root.gameObject);
                return;
            }

            if (FindInstance(gameObject.scene, out var instance) && instance != this)
            {
                DestroyImmediate(gameObject);
                return;
            }

            base.Awake();
        }
    }
}
