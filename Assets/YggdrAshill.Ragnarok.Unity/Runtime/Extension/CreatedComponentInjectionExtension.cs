#nullable enable
using System;
using UnityEngine;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public static class CreatedComponentInjectionExtension
    {
        public static IInstanceInjection Under(this ICreatedComponentInjection injection, Func<Transform> getParentTransform)
        {
            var parentTransform = new ParentTransformToReturnCache(getParentTransform);

            return injection.Under(parentTransform);
        }
        
        public static IInstanceInjection Under(this ICreatedComponentInjection injection, Transform parent)
        {
            var parentTransform = new ParentTransformToReturnInstance(parent);
            
            return injection.Under(parentTransform);
        }
    }
}
