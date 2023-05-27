using UnityEngine;

namespace YggdrAshill.Ragnarok.Unity.Samples
{
    internal interface IOutputReceiver
    {
        void ReceiveOutput(Vector3 velocity);
    }
}
