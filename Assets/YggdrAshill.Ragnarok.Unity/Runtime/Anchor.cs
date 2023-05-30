#nullable enable
using YggdrAshill.Ragnarok.Unity;
using System;
using UnityEngine;

namespace YggdrAshill.Ragnarok
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
