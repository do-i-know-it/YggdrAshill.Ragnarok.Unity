using YggdrAshill.Ragnarok.Experimental;
using UnityEngine;

namespace YggdrAshill.Ragnarok.Unity.Samples
{
    internal sealed class MovementInstallation : MonoInstallation
    {
        [SerializeField] private GameObject instance;

        public override void Install(IContainer container)
        {
            container.RegisterComponent<IMovement>(instance);
        }
    }
}
