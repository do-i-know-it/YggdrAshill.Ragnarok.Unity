#nullable enable
using System;
using UnityEngine;

namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class PropertyInjectionServiceComponentInstallation : MonoInstallation
    {
        [SerializeField] private PropertyInjectionServiceComponent? propertyInjectionService;
        private PropertyInjectionServiceComponent PropertyInjectionService
        {
            get
            {
                if (propertyInjectionService == null)
                {
                    throw new InvalidOperationException($"{nameof(propertyInjectionService)} is null.");
                }

                return propertyInjectionService;
            }
        }
        
        public override void Install(IObjectContainer container)
        {
            container.RegisterComponent(PropertyInjectionService).WithPropertyInjection();
        }
    }
}
