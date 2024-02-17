#nullable enable
namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public interface IUnityFactoryResolution : IFactoryResolution
    {
        IFactoryResolution Under(IParentTransform parent);
    }
}
