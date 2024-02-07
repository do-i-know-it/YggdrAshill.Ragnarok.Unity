#nullable enable
using UnityEngine;

namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class SceneLifecycleSpecification : MonoInstallation
    {
        [SerializeField] private GameObject? target;
        private GameObject Target
        {
            get
            {
                if (target == null)
                {
                    target = transform.gameObject;
                }

                return target;
            }
        }

        [SerializeField] private string sceneName = string.Empty;
        
        public override void Install(IObjectContainer container)
        {
            container.RegisterInstance(Target);
            container.Register<LoadAdditiveSceneWithSceneLifecycle>(Lifetime.Global).WithArgument(sceneName);
            container.Register(resolver => resolver.Resolve<LoadAdditiveSceneWithSceneLifecycle>().Execute());
        }
    }
}
