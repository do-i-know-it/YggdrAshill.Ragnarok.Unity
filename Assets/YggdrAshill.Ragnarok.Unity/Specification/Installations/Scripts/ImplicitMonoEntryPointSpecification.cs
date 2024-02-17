#nullable enable
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok.Specification
{
    public class ImplicitMonoEntryPointSpecification : MonoInstallation
    {
        public override void Install(IObjectContainer container)
        {
            container.RegisterCallback<IEnumerable<HelloWorldWithGameObject>>(serviceList =>
            {
                UnityEngine.Debug.LogError($"{nameof(ImplicitMonoEntryPointSpecification)} in {gameObject}.");
                
                foreach (var service in serviceList)
                {
                    service.Execute();
                }
            });
        }
    }
}
