#nullable enable
using UnityEngine;

namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class ImplicitMonoEntryPointInstallation : MonoInstallation
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
            container.Register<HelloWorldWithGameObject>(Lifetime.Temporal).WithArgument(Instance);
        }
    }
}
