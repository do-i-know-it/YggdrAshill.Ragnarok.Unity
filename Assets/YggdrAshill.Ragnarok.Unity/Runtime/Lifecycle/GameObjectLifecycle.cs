#nullable enable
using YggdrAshill.Ragnarok.Experimental;
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
            var target = transform.parent;

            while (target != null)
            {
                if (target.TryGetComponent<Lifecycle>(out var lifecycle))
                {
                    return lifecycle.CreateChildContext();
                }

                target = target.parent;
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
    }
}
