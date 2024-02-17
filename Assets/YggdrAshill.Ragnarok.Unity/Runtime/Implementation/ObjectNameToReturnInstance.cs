#nullable enable
namespace YggdrAshill.Ragnarok
{
    internal sealed class ObjectNameToReturnInstance : IObjectName
    {
        private readonly string name;

        public ObjectNameToReturnInstance(string name)
        {
            this.name = name;
        }
        
        public string? GetObjectName()
        {
            return name;
        }
    }
}
