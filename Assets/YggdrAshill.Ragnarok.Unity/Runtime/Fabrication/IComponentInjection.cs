#nullable enable
namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public interface IComponentInjection : IInstanceInjection
    {
        IInstanceInjection Under(IAnchor anchor);
        
        IInstanceInjection DontDestroyOnLoad();
    }
}
