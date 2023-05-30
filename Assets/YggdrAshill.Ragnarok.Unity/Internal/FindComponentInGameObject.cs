#nullable enable
using System;
using UnityEngine;

namespace YggdrAshill.Ragnarok.Unity.Internal
{
    internal sealed class FindComponentInGameObject :
        IInstantiation
    {
        private readonly Type componentType;
        private readonly GameObject instance;
        private readonly IInjection? injection;

        public FindComponentInGameObject(Type componentType, GameObject instance, IInjection? injection)
        {
            this.componentType = componentType;
            this.instance = instance;
            this.injection = injection;
        }
        
        public object Instantiate(IResolver resolver)
        {
            var component = instance.GetComponentInChildren(componentType, true);

            if (component == null)
            {
                throw new RagnarokException(componentType, $"{componentType} is not in {instance}.");
            }

            injection?.Inject(resolver, component);
            
            return component;
        }
    }
}
