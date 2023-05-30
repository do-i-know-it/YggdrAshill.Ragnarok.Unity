using UnityEngine;

namespace YggdrAshill.Ragnarok.Unity.Samples
{
    internal sealed class ChildSceneEntryPoint : MonoEntryPoint
    {
        [SerializeField] private Vector2 inputOffset;
        [SerializeField] private Vector3 outputOffset;
        
        protected override void Configure(IContainer container)
        {
            container.RegisterComponentOnNewGameObject<IInputOffset, InputOffset>(Lifetime.Global)
                .Under(transform)
                .WithFieldsInjected()
                .From("offset", inputOffset);
            container.RegisterComponentOnNewGameObject<IOutputOffset, OutputOffset>(Lifetime.Global)
                .Under(transform)
                .WithFieldsInjected()
                .From("offset", outputOffset);
        }
    }
}
