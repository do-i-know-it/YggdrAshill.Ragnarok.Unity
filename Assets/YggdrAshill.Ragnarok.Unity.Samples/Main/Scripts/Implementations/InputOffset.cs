using UnityEngine;

namespace YggdrAshill.Ragnarok.Unity.Samples
{
    internal sealed class InputOffset : MonoBehaviour,
        IInputOffset
    {
        [SerializeField]
        [InjectField]
        private Vector2 offset;
        public Vector2 Offset => offset;
    }
}
