#nullable enable
using YggdrAshill.Ragnarok.Experimental;
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

        protected override IObjectContext GetCurrentContext()
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
