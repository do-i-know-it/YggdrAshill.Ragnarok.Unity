#nullable enable
using YggdrAshill.Ragnarok.Experimental;
using UnityEngine;

namespace YggdrAshill.Ragnarok.Unity.Specification
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
            container.RegisterComponent(Instance);
            container.Register<HelloWorldWithGameObject>(Lifetime.Global);
            container.Register(resolver => resolver.Resolve<HelloWorldWithGameObject>().Execute());
        }
    }
}
