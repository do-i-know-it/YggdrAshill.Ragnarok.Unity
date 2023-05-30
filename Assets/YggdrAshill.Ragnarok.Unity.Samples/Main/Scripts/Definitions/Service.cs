using UnityEngine;

namespace YggdrAshill.Ragnarok.Unity.Samples
{
    internal sealed class Service :
        IPreUpdatable
    {
        private readonly IInputSender inputSender;
        private readonly IInputOffset inputOffset;
        private readonly IMovement movement;
        private readonly IOutputOffset outputOffset;
        private readonly IOutputReceiver outputReceiver;

        [Inject]
        public Service(IInputSender inputSender, IInputOffset inputOffset, IMovement movement, IOutputOffset outputOffset, IOutputReceiver outputReceiver)
        {
            this.inputSender = inputSender;
            this.inputOffset = inputOffset;
            this.movement = movement;
            this.outputOffset = outputOffset;
            this.outputReceiver = outputReceiver;
        }

        public void PreUpdate()
        {
            var input = inputSender.SendInput();
            var correctedInput = input + inputOffset.Offset;
            var output = movement.CalculateVelocity(correctedInput);
            var correctedOutput = output + outputOffset.Offset;
            outputReceiver.ReceiveOutput(correctedOutput);
            
            Debug.Log($"{nameof(input)}={input}, {nameof(correctedInput)}={correctedInput}, {nameof(output)}={output}, {nameof(correctedOutput)}={correctedOutput}.");
        }
    }
}
