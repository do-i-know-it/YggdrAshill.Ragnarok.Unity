#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    [DefaultExecutionOrder(LifecycleExecutionOrder.Project)]
    public sealed class ProjectLifecycle : Lifecycle
    {
        private static readonly object lockObject = new();
        private static volatile ProjectLifecycle? instance;
        public static bool FindInstance(out ProjectLifecycle lifecycle)
        {
            lifecycle = default!;
            
            lock (lockObject)
            {
                if (instance != null)
                {
                    lifecycle = instance;

                    return true;
                }

                if (RagnarokConfiguration.ProjectLifecycle != null)
                {
                    lifecycle = instance = Instantiate(RagnarokConfiguration.ProjectLifecycle);

                    return true;
                }
            }
            
            return false;
        }

        protected override bool RunAutomatically => true;

        protected override IObjectContext GetCurrentContext()
        {
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

            lock (lockObject)
            {
                if (instance == null)
                {
                    instance = this;
                }
                else
                {
                    DestroyImmediate(gameObject);
                    return;
                }
            }

            DontDestroyOnLoad(gameObject);

            base.Awake();
        }

        protected override void OnDestroy()
        {
            lock (lockObject)
            {
                if (instance != this)
                {
                    return;
                }

                instance = default;
            }
        }
    }
}
