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
        public static GameObjectLifecycle Create(Transform? parent = null, params IInstallation[] installationList)
        {
            var instance = new GameObject($"{nameof(GameObjectLifecycle)}");
            
            instance.SetActive(false);

            if (parent != null)
            {
                instance.transform.SetParent(parent, false);
            }

            var component = instance.AddComponent<GameObjectLifecycle>();
            
            component.installationList.AddRange(installationList);
            
            instance.SetActive(true);

            return component;
        }
        
        public static GameObjectLifecycle Create(GameObjectLifecycle prefab, Transform? parent = null, params IInstallation[] installationList)
        {
            var wasActive = prefab.gameObject.activeSelf;
            
            if (wasActive)
            {
                prefab.gameObject.SetActive(false);
            }
            
            var component = Instantiate(prefab);

            if (parent != null)
            {
                component.transform.SetParent(parent, false);
            }
            
            component.installationList.AddRange(installationList);
            
            if (wasActive)
            {
                prefab.gameObject.SetActive(true);
                component.gameObject.SetActive(true);
            }

            return component;
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

        private readonly List<IInstallation> installationList = new();

        protected override IEnumerable<IInstallation> GetInstallationList()
        {
            return installationList.Concat(ScriptableInstallationList).Concat(MonoInstallationList);
        }
    }
}
