#nullable enable
using System;
using UnityEngine;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public static class UnityFactoryResolutionExtension
    {
        public static IFactoryResolution Under(this IUnityFactoryResolution resolution, Func<Transform> anchor)
        {
            var anchorTransform = new AnchorTransform(anchor);

            return resolution.Under(anchorTransform);
        }
        
        public static IFactoryResolution Under(this IUnityFactoryResolution resolution, Transform parent)
        {
            var anchorTransform = new AnchorTransform(parent);

            return resolution.Under(anchorTransform);
        }
    }
}
