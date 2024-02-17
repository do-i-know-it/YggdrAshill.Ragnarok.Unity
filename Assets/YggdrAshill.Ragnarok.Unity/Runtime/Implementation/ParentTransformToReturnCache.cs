#nullable enable
using System;
using UnityEngine;

namespace YggdrAshill.Ragnarok
{
    internal sealed class ParentTransformToReturnCache : IParentTransform
    {
        private readonly Lazy<Transform> cache;

        public ParentTransformToReturnCache(Func<Transform> getParentTransform)
        {
            cache = new Lazy<Transform>(getParentTransform);
        }
        
        public Transform? GetParentTransform()
        {
            return cache.Value;
        }
    }
}
