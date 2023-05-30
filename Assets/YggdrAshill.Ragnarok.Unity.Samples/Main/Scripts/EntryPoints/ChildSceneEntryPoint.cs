using UnityEngine;

namespace YggdrAshill.Ragnarok.Unity.Samples
{
    internal sealed class ChildSceneEntryPoint : MonoEntryPoint
    {
        [SerializeField] private Vector2 inputOffset;
        [SerializeField] private Vector3 outputOffset;
        
        protected override void Configure(IContainer container)
        {
            container.RegisterGlobal<IInputOffset, InputOffset>()
                .Under(transform)
                .WithFieldsInjected()
                .From("offset", inputOffset);
            container.RegisterGlobal<IOutputOffset, OutputOffset>()
                .Under(transform)
                .WithFieldsInjected()
                .From("offset", outputOffset);
        }
    }
}
