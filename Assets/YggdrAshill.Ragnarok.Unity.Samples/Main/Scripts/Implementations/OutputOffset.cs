using UnityEngine;

namespace YggdrAshill.Ragnarok.Unity.Samples
{
    internal sealed class OutputOffset : MonoBehaviour,
        IOutputOffset
    {
        [SerializeField]
        [InjectField]
        private Vector3 offset;
        public Vector3 Offset => offset;
    }
}
