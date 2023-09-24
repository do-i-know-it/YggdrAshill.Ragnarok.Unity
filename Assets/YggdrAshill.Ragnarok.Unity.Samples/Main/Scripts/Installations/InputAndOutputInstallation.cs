using YggdrAshill.Ragnarok.Experimental;
using UnityEngine;

namespace YggdrAshill.Ragnarok.Unity.Samples
{
    internal sealed class InputAndOutputInstallation : MonoInstallation
    {
        [SerializeField] private Vector2 inputOffset;
        [SerializeField] private Vector3 outputOffset;
        
        [SerializeField] private InputSender inputSender;
        [SerializeField] private OutputReceiver outputReceiver;
        
        public override void Install(IContainer container)
        {
            container.RegisterComponentInNewPrefab<IInputSender, InputSender>(inputSender, Lifetime.Global);
            container.RegisterComponentInNewPrefab<IOutputReceiver, OutputReceiver>(outputReceiver, Lifetime.Global);
            
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
