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

        [SerializeField] private SceneLifecycle? rootLifecycle;
        protected override IContext GetCurrentContext()
        {
            if (rootLifecycle != null)
            {
                return rootLifecycle.CreateChildContext();
            }
                
            var parent = transform.parent;

            while (parent != null)
            {
                if (parent.TryGetComponent<Lifecycle>(out var lifecycle))
                {
                    return lifecycle.CreateChildContext();
                }
                
                parent = parent.parent;
            }
            
            if (SceneLifecycle.ParentLifecycle != null)
            {
                return SceneLifecycle.ParentLifecycle.CreateChildContext();
            }
            
            if (ProjectLifecycle.Instance != null)
            {
                return ProjectLifecycle.Instance.CreateChildContext();
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
