#nullable enable
using UnityEngine;

namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class ProjectLifecycleSpecification : MonoInstallation
    {
        [SerializeField] private GameObject? instance;
        private GameObject Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = transform.gameObject;
                }

                return instance;
            }
        }
        
        public override void Install(IObjectContainer container)
        {
            container.RegisterInstance(Instance);
            container.Register<HelloWorldWithGameObject>(Lifetime.Global);
            container.Register(resolver => resolver.Resolve<HelloWorldWithGameObject>().Execute());
        }
    }
}
