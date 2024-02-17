#nullable enable
namespace YggdrAshill.Ragnarok
{
    internal sealed class ObjectNameToReturnNothing : IObjectName
    {
        public static ObjectNameToReturnNothing Instance { get; } = new();

        private ObjectNameToReturnNothing()
        {
            
        }
        
        public string? GetObjectName()
        {
            return null;
        }
    }
}
