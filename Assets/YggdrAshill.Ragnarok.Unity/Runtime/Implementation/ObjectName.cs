#nullable enable
using System;

namespace YggdrAshill.Ragnarok
{
    internal sealed class ObjectName : IObjectName
    {
        private readonly Func<string> getName;

        public ObjectName(Func<string> getName)
        {
            this.getName = getName;
        }

        public ObjectName(string name) : this(() => name)
        {
            
        }
        
        public string GetName()
        {
            return getName.Invoke();
        }
    }
}
