#nullable enable
namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public interface INamedComponentInjection : ICreatedComponentInjection
    {
        ICreatedComponentInjection Named(IObjectName name);
    }
}
