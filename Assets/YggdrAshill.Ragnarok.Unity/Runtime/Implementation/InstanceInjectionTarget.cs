using System;

namespace YggdrAshill.Ragnarok
{
    [Flags]
    public enum InstanceInjectionTarget : byte
    {
        None = 0,
        Field = 1 << 0,
        Property = 1 << 1,
        Method = 1 << 2,
    }
}
