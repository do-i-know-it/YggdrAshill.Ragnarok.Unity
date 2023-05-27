using UnityEngine;

namespace YggdrAshill.Ragnarok.Unity.Samples
{
    internal sealed class InputSender : MonoBehaviour,
        IInputSender
    {
        [SerializeField] private KeyCode upward = KeyCode.W;
        [SerializeField] private KeyCode downward = KeyCode.S;
        [SerializeField] private KeyCode leftward = KeyCode.A;
        [SerializeField] private KeyCode rightward = KeyCode.D;
        
        public Vector2 SendInput()
        {
            var left = Input.GetKey(leftward) ? -1 : 0;
            var right = Input.GetKey(rightward) ? 1 : 0;
            var horizontal = left + right;

            var up = Input.GetKey(upward) ? 1 : 0;
            var down = Input.GetKey(downward) ? -1 : 0;
            var vertical = up + down;

            return new Vector2(horizontal, vertical);
        }
    }
}
