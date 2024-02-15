#nullable enable
using System;
using UnityEngine;

namespace YggdrAshill.Ragnarok
{
    internal sealed class InstantiateToReturnComponentInSibling : IInstantiation
    {
        private readonly GameObject instance;
        private readonly Type type;
        private readonly bool includeInactive;

        public InstantiateToReturnComponentInSibling(GameObject instance, Type type, bool includeInactive)
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
            
            var parentTransform = instance.transform.parent;

            if (parentTransform != null)
            {
                for (var index = 0; index < parentTransform.childCount; index++)
                {
                    var child = parentTransform.GetChild(index);

                    if ((includeInactive || child.gameObject.activeSelf) && child.TryGetComponent(type, out component))
                    {
                        return component;
                    }
                }
            }
            
            throw new RagnarokException(type, $"{type} is not in {instance}.");
        }
    }
}
