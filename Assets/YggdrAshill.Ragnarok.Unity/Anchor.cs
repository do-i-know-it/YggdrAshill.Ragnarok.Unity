#nullable enable
using YggdrAshill.Ragnarok.Unity.Internal;
using System;
using UnityEngine;

namespace YggdrAshill.Ragnarok.Unity
{
    internal sealed class Anchor :
        IAnchor
    {
        private readonly Func<Transform> getParentTransform;

        public Anchor(Func<Transform> getParentTransform)
        {
            this.getParentTransform = getParentTransform;
        }

        public Anchor(Transform parentTransform) : this(() => parentTransform)
        {
            
        }

        public Transform GetParentTransform()
        {
            return getParentTransform.Invoke();
        }
    }
}
