#nullable enable
namespace YggdrAshill.Ragnarok
{
    public interface IUnitySubContainerResolution : ISubContainerResolution
    {
        // TODO: use Lifecycle, instead of IAnchor?
        ITypeAssignment Under(IParentTransform parent);
    }
}
