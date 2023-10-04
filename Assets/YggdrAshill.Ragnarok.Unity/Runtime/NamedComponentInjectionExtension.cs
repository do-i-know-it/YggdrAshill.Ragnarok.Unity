#nullable enable
using System;
using UnityEngine;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public static class NamedComponentInjectionExtension
    {
        public static IInstanceInjection Under(this INamedComponentInjection injection, Func<Transform> anchor)
        {
            return injection.Under(new Anchor(anchor));
        }
        
        public static IInstanceInjection Under(this INamedComponentInjection injection, Transform parent)
        {
            return injection.Under(new Anchor(parent));
        }
    }
}
