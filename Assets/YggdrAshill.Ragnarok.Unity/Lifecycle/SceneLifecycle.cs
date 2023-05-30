#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace YggdrAshill.Ragnarok
{
    [DefaultExecutionOrder(LifecycleExecutionOrder.Scene)]
    public sealed class SceneLifecycle : Lifecycle
    {
        private static Stack<SceneLifecycle> ParentLifecycleStack { get; } = new Stack<SceneLifecycle>();

        public static SceneLifecycle? ParentLifecycle
        {
            get
            {
                if (ParentLifecycleStack.Count == 0)
                {
                    return null;
                }

                return ParentLifecycleStack.Peek();
            }
        }

        public readonly struct OverrideParentLifecycleScope :
            IDisposable
        {
            private readonly bool isInitialized;
            
            internal OverrideParentLifecycleScope(SceneLifecycle lifecycle)
            {
                ParentLifecycleStack.Push(lifecycle);
                isInitialized = true;
            }

            public void Dispose()
            {
                if (isInitialized)
                {
                    throw new Exception($"{nameof(OverrideParentLifecycleScope)}");
                }
                
                ParentLifecycleStack.Pop();
            }
        }

        public OverrideParentLifecycleScope OverrideParentLifecycle()
        {
            return new OverrideParentLifecycleScope(this);
        }
        
        [SerializeField] private bool runAutomatically = true;
        protected override bool RunAutomatically => runAutomatically;
        
        [SerializeField] private SceneLifecycle? parentLifecycle;
        protected override IContext GetCurrentContext()
        {
            if (parentLifecycle != null)
            {
                if (parentLifecycle == this)
                {
                    throw new Exception("parent life cycle is own.");
                }
                
                return parentLifecycle.CreateChildContext();
            }

            if (ParentLifecycle != null)
            {
                return ParentLifecycle.CreateChildContext();
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

        protected override void Awake()
        {
            if (transform.parent != null)
            {
                DestroyImmediate(this);
            }
            
            base.Awake();
        }
    }
}
