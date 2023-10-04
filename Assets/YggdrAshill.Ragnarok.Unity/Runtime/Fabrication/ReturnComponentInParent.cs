#nullable enable
using System;
using UnityEngine;

namespace YggdrAshill.Ragnarok
{
    internal sealed class ReturnComponentInParent : IInstantiation
    {
        private readonly GameObject instance;
        private readonly Type type;
        private readonly bool includeInactive;
        private readonly IInjection? injection;

        public ReturnComponentInParent(GameObject instance, Type type, bool includeInactive, IInjection? injection)
        {
            this.instance = instance;
            this.type = type;
            this.includeInactive = includeInactive;
            this.injection = injection;
        }
        
        public object Instantiate(IObjectResolver resolver)
        {
            var component = instance.GetComponentInParent(type, includeInactive);

            if (component == null)
            {
                throw new RagnarokException(type, $"{type} is not in {instance}.");
            }
            
            injection?.Inject(resolver, component);
            
            return component;
        }
    }
}
