using System;
using UnityEngine;

namespace YggdrAshill.Ragnarok.Unity.Samples
{
    internal sealed class Movement : MonoBehaviour,
        IMovement
    {
        [SerializeField] private float velocity = 5.0f;
        [SerializeField] private Direction direction;
        
        public Vector3 CalculateVelocity(Vector2 input)
        {
            switch (direction)
            {
                case Direction.Forward:
                    return new Vector3(input.x, 0, input.y) * velocity;
                case Direction.Upward:
                    return input * velocity;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
