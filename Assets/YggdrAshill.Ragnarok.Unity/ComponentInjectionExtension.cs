#nullable enable
using YggdrAshill.Ragnarok.Unity.Internal;
using YggdrAshill.Ragnarok.Fabrication;
using System;
using UnityEngine;

namespace YggdrAshill.Ragnarok.Unity
{
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
