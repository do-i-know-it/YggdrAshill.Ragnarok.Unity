#nullable enable
using System;
using UnityEngine;

namespace YggdrAshill.Ragnarok
{
    internal sealed class InstantiateToReturnComponentInChildren : IInstantiation
    {
        private readonly GameObject instance;
        private readonly Type type;
        private readonly bool includeInactive;

        public InstantiateToReturnComponentInChildren(GameObject instance, Type type, bool includeInactive)
        {
            this.instance = instance;
            this.type = type;
            this.includeInactive = includeInactive;
        }
        
        public object Instantiate(IObjectResolver resolver)
        {
            var component = instance.GetComponentInChildren(type, includeInactive);

            if (component == null)
            {
                throw new RagnarokException(type, $"{type} is not in {instance}.");
            }

            return component;
        }
    }
}
