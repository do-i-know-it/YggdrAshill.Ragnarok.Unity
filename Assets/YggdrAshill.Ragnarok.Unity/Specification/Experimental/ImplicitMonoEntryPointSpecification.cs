#nullable enable
using YggdrAshill.Ragnarok.Experimental;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok.Specification
{
    public class ImplicitMonoEntryPointSpecification : MonoInstallation
    {
        public override void Install(IObjectContainer container)
        {
            container.Register(resolver =>
            {
                UnityEngine.Debug.LogError($"{nameof(ImplicitMonoEntryPointSpecification)} in {gameObject}.");
                var serviceList = resolver.Resolve<IEnumerable<HelloWorldWithGameObject>>();
                
                foreach (var service in serviceList)
                {
                    service.Execute();
                }
            });
        }
    }
}
