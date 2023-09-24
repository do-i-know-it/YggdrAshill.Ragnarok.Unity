#nullable enable
using UnityEngine;

namespace YggdrAshill.Ragnarok.Experimental
{
    public abstract class MonoInstallation : MonoBehaviour,
        IInstallation
    {
        public abstract void Install(IContainer container);
    }
}
