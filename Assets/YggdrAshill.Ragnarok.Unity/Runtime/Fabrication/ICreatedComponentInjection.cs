#nullable enable
namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public interface ICreatedComponentInjection : IInstanceInjection
    {
        IInstanceInjection Under(IAnchorTransform anchor);
        
        IInstanceInjection DontDestroyOnLoad();
    }
}
