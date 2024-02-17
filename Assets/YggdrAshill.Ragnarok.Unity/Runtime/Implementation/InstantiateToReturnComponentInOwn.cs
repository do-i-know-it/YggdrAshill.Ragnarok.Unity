#nullable enable
using System;
using UnityEngine;

namespace YggdrAshill.Ragnarok
{
    internal sealed class InstantiateToReturnComponentInOwn : IInstantiation
    {
        private readonly GameObject instance;
        private readonly Type type;
        private readonly bool includeInactive;

        public InstantiateToReturnComponentInOwn(GameObject instance, Type type, bool includeInactive)
        {
            this.instance = instance;
            this.type = type;
            this.includeInactive = includeInactive;
        }
        
        public object Instantiate(IObjectResolver resolver)
        {
            if ((includeInactive || instance.activeSelf) && instance.TryGetComponent(type, out var component))
            {
                return component;
            }
            
            throw new RagnarokException(type, $"{type} is not in {instance}.");
        }
    }
}
