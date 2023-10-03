#nullable enable
using System;
using UnityEngine;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public static class ComponentInjectionExtension
    {
        public static IInstanceInjection Under(this IComponentInjection injection, Func<Transform> anchor)
        {
            return injection.Under(new Anchor(anchor));
        }
        
        public static IInstanceInjection Under(this IComponentInjection injection, Transform parentTransform)
        {
            return injection.Under(new Anchor(parentTransform));
        }
    }
}
