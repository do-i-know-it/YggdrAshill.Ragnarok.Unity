#nullable enable
using System;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public static class NamedComponentInjectionExtension
    {
        public static ICreatedComponentInjection Named(this INamedComponentInjection injection, Func<string> getObjectName)
        {
            var objectName = new ObjectNameToReturnCache(getObjectName);
            
            return injection.Named(objectName);
        }
        
        public static ICreatedComponentInjection Named(this INamedComponentInjection injection, string name)
        {
            var objectName = new ObjectNameToReturnInstance(name);
            
            return injection.Named(objectName);
        }
    }
}
