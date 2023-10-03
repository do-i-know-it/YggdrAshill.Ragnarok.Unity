#nullable enable
using UnityEngine;

namespace YggdrAshill.Ragnarok.Experimental
{
    // TODO: add document comments.
    public abstract class MonoInstallation : MonoBehaviour, IInstallation
    {
        public abstract void Install(IObjectContainer container);
    }
}
