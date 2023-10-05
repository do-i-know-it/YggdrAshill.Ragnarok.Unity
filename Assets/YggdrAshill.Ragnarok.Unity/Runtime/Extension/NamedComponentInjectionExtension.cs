#nullable enable
using System;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public static class NamedComponentInjectionExtension
    {
        public static ICreatedComponentInjection Named(this INamedComponentInjection injection, Func<string> objectName)
        {
            return injection.Named(new ObjectName(objectName));
        }
        
        public static ICreatedComponentInjection Named(this INamedComponentInjection injection, string objectName)
        {
            return injection.Named(new ObjectName(objectName));
        }
    }
}
