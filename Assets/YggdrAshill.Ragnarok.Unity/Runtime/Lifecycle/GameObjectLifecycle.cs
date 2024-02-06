#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    [DefaultExecutionOrder(LifecycleExecutionOrder.GameObject)]
    public sealed class GameObjectLifecycle : Lifecycle
    {
        public static GameObjectLifecycle Create(GameObjectLifecycle prefab, Transform? parent = null)
        {
            var wasActive = prefab.gameObject.activeSelf;
            
            if (wasActive)
            {
                prefab.gameObject.SetActive(false);
            }
            
            var instance = Instantiate(prefab);

            if (parent != null)
            {
                instance.transform.SetParent(parent, false);
            }
            
            if (wasActive)
            {
                prefab.gameObject.SetActive(true);
                instance.gameObject.SetActive(true);
            }

            return instance;
        }

        [SerializeField] private bool runAutomatically = true;
        protected override bool RunAutomatically => runAutomatically;

        protected override IObjectContext GetCurrentContext()
        {
            var target = transform.parent;

            while (target != null)
            {
                if (target.TryGetComponent<Lifecycle>(out var lifecycle))
                {
                    return lifecycle.CreateContext();
                }

                target = target.parent;
            }

            if (SceneLifecycle.FindInstance(gameObject.scene, out var sceneLifecycle))
            {
                return sceneLifecycle.CreateContext();
            }

            if (SceneLifecycle.FindOverriddenLifecycle(out sceneLifecycle))
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
    }
}
