using UnityEngine;

namespace YggdrAshill.Ragnarok.Unity.Samples
{
    internal interface IMovement
    {
        Vector3 CalculateVelocity(Vector2 input);
    }
}
