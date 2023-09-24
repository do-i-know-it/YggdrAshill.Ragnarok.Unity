#nullable enable
using YggdrAshill.Ragnarok.Experimental;
using UnityEngine;

namespace YggdrAshill.Ragnarok.Unity.Specification
{
    internal sealed class SceneLifecycleHelloWorldInstallation : MonoInstallation
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
        
        public override void Install(IContainer container)
        {
            container.Register<HelloWorld>(Lifetime.Global);
            container.Register(resolver => resolver.Resolve<HelloWorld>().Execute());
        }
    }
}
