#nullable enable
namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public interface ISearchedComponentInjection : IInstanceInjection
    {
        IInstanceInjection IncludeInactive();
    }
}
