using UnityEngine;

namespace YggdrAshill.Ragnarok.Unity.Samples
{
    internal sealed class OutputReceiver : MonoBehaviour,
        IOutputReceiver
    {
        [SerializeField] private Transform targetTransform;
        private Transform TargetTransform
        {
            get
            {
                if (targetTransform == null)
                {
                    targetTransform = transform;
                }

                return targetTransform;
            }
        }
        
        public void ReceiveOutput(Vector3 velocity)
        {
            TargetTransform.position += velocity * Time.deltaTime;
        }
    }
}
