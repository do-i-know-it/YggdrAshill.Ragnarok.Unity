#nullable enable
using YggdrAshill.Ragnarok.Experimental;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace YggdrAshill.Ragnarok
{
    [DefaultExecutionOrder(LifecycleExecutionOrder.Project)]
    internal sealed class ProjectLifecycle : Lifecycle
    {
        private static readonly object lockObject = new object();
        private static volatile ProjectLifecycle? instance;
        public static ProjectLifecycle? Instance
        {
            get
            {
                lock (lockObject)
                {
                    if (instance != null)
                    {
                        return instance;
                    }

                    if (RagnarokConfiguration.ProjectLifecycle != null)
                    {
                        return instance = Instantiate(RagnarokConfiguration.ProjectLifecycle);
                    }

                    return null;
                }
            }
        }

        protected override bool RunAutomatically => true;

        protected override IContext GetCurrentContext()
        {
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
                DestroyImmediate(this);
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
                    DestroyImmediate(this);
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
