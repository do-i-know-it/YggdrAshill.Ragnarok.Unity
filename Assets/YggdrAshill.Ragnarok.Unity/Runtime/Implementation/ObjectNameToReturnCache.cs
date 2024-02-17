#nullable enable
using System;

namespace YggdrAshill.Ragnarok
{
    internal sealed class ObjectNameToReturnCache : IObjectName
    {
        private readonly Lazy<string> cache;

        public ObjectNameToReturnCache(Func<string> getObjectName)
        {
            cache = new Lazy<string>(getObjectName);
        }

        public string? GetObjectName()
        {
            return cache.Value;
        }
    }
}
