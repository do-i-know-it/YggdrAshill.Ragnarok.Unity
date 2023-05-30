#nullable enable
namespace YggdrAshill.Ragnarok.Unity
{
    public interface IComponentInjection :
        IInstanceInjection
    {
        IInstanceInjection Under(IAnchor anchor);
        
        IInstanceInjection DontDestroyOnLoad();
    }
}
