#nullable enable
namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public interface INamedComponentInjection : IInstanceInjection
    {
        IInstanceInjection Under(IAnchor anchor);
        
        IInstanceInjection DontDestroyOnLoad();
    }
}
