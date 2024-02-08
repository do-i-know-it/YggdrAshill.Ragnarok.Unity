#nullable enable
using System;
using UnityEngine;

namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class MethodInjectionServiceComponentInstallation : MonoInstallation
    {
        [SerializeField] private MethodInjectionServiceComponent? methodInjectionService;
        private MethodInjectionServiceComponent MethodInjectionService
        {
            get
            {
                if (methodInjectionService == null)
                {
                    throw new InvalidOperationException($"{nameof(methodInjectionService)} is null.");
                }

                return methodInjectionService;
            }
        }
        
        public override void Install(IObjectContainer container)
        {
            container.RegisterComponent(MethodInjectionService).WithMethodInjection();
        }
    }
}
