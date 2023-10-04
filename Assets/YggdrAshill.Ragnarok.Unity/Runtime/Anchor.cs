#nullable enable
using System;
using UnityEngine;

namespace YggdrAshill.Ragnarok
{
    internal sealed class Anchor : IAnchor
    {
        private readonly Func<Transform> getTransform;

        public Anchor(Func<Transform> getTransform)
        {
            this.getTransform = getTransform;
        }

        public Anchor(Transform parent) : this(() => parent)
        {
            
        }

        public Transform GetTransform()
        {
            return getTransform.Invoke();
        }
    }
}
