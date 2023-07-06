#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace YggdrAshill.Ragnarok
{
    [DefaultExecutionOrder(LifecycleExecutionOrder.GameObject)]
    public sealed class GameObjectLifecycle : Lifecycle
    {
        [SerializeField] private bool runAutomatically = true;
        protected override bool RunAutomatically => runAutomatically;

        protected override IContext GetCurrentContext()
        {
            var parent = transform.parent;

            while (parent != null)
            {
                if (parent.TryGetComponent<Lifecycle>(out var lifecycle))
                {
                    return lifecycle.CreateChildContext();
                }

                parent = parent.parent;
            }

            var sceneLifecycle = SceneLifecycle.InstanceOf(gameObject.scene);
            if (sceneLifecycle != null)
            {
                return sceneLifecycle.CreateChildContext();
            }

            sceneLifecycle = SceneLifecycle.OverriddenLifecycle;
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

        [SerializeField] private ScriptableEntryPoint[] scriptableEntryPointList = Array.Empty<ScriptableEntryPoint>();
        [SerializeField] private MonoEntryPoint[] monoEntryPointList = Array.Empty<MonoEntryPoint>();
        protected override IEnumerable<IEntryPoint> GetEntryPointList()
        {
            return ((IEnumerable<IEntryPoint>)scriptableEntryPointList).Concat(monoEntryPointList);
        }
    }
}
