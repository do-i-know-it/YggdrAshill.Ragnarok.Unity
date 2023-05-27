#nullable enable
using YggdrAshill.Ragnarok.Fabrication;

namespace YggdrAshill.Ragnarok.Unity.Internal
{
    public interface IComponentInjection :
        IInstanceInjection
    {
        IInstanceInjection Under(IAnchor anchor);
        
        IInstanceInjection DontDestroyOnLoad();
    }
}
