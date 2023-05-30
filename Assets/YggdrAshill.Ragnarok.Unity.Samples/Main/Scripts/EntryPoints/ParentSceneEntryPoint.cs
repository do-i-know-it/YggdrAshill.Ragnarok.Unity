using UnityEngine;

namespace YggdrAshill.Ragnarok.Unity.Samples
{
    internal sealed class ParentSceneEntryPoint : MonoEntryPoint
    {
        [SerializeField] private InputSender inputSender;
        [SerializeField] private OutputReceiver outputReceiver;
        
        protected override void Configure(IContainer container)
        {
            container.RegisterComponent<IInputSender>(inputSender);
            container.RegisterComponent<IOutputReceiver>(outputReceiver);
        }
    }
}
