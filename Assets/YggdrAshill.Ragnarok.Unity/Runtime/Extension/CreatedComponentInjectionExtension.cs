#nullable enable
using System;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public static class CreatedComponentInjectionExtension
    {
        public static INamedComponentInjection Named(this ICreatedComponentInjection injection, Func<string> objectName)
        {
            return injection.Named(new ObjectName(objectName));
        }
        
        public static INamedComponentInjection Named(this ICreatedComponentInjection injection, string objectName)
        {
            return injection.Named(new ObjectName(objectName));
        }
    }
}
