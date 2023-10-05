#nullable enable
using System;
using UnityEngine;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public static class CreatedComponentInjectionExtension
    {
        public static IInstanceInjection Under(this ICreatedComponentInjection injection, Func<Transform> anchor)
        {
            return injection.Under(new Anchor(anchor));
        }
        
        public static IInstanceInjection Under(this ICreatedComponentInjection injection, Transform parent)
        {
            return injection.Under(new Anchor(parent));
        }
    }
}
