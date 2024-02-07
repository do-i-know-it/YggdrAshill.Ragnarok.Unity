#nullable enable
using System;
using UnityEngine;

namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class FieldInjectionServiceComponentInstallation : MonoInstallation
    {
        [SerializeField] private FieldInjectionServiceComponent? fieldInjectionService;
        private FieldInjectionServiceComponent FieldInjectionService
        {
            get
            {
                if (fieldInjectionService == null)
                {
                    throw new InvalidOperationException($"{nameof(fieldInjectionService)} is null.");
                }

                return fieldInjectionService;
            }
        }
        
        public override void Install(IObjectContainer container)
        {
            container.RegisterComponent(FieldInjectionService).WithFieldInjection();
        }
    }
}
