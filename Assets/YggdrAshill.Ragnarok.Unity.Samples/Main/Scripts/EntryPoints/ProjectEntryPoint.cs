using UnityEngine;

namespace YggdrAshill.Ragnarok.Unity.Samples
{
    internal sealed class ProjectEntryPoint : MonoEntryPoint
    {
        [SerializeField] private GameObject instance;
        
        protected override void Configure(IContainer container)
        {
            container.RegisterComponent<IMovement>(instance);
        }
    }
}
