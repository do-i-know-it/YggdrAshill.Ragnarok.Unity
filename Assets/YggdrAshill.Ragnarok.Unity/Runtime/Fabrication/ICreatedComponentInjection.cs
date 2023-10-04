#nullable enable
namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public interface ICreatedComponentInjection : INamedComponentInjection
    {
        INamedComponentInjection Named(IObjectName name);
    }
}
