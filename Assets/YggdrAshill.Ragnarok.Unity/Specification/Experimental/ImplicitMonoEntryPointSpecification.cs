#nullable enable
using YggdrAshill.Ragnarok.Experimental;
using System.Collections.Generic;
using UnityEngine;

namespace YggdrAshill.Ragnarok.Unity.Specification
{
    public class ImplicitMonoEntryPointSpecification : MonoInstallation
    {
        private sealed class HelloWorld
        {
            private readonly GameObject target;

            [Inject]
            public HelloWorld(GameObject target)
            {
                this.target = target;
            }
            
            public void Execute()
            {
                Debug.LogError($"Hello world in {target.name}.");
            }
        }
        
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
        
        public override void Install(IObjectContainer container)
        {
            container.Register<HelloWorld>(Lifetime.Temporal).WithArgument(nameof(target), Target);
            container.Register(resolver =>
            {
                foreach (var service in resolver.Resolve<IEnumerable<HelloWorld>>())
                {
                    service.Execute();
                }
            });
        }
    }
}
