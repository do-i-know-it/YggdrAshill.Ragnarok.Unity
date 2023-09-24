#nullable enable
using UnityEngine;

namespace YggdrAshill.Ragnarok.Experimental
{
    public abstract class ScriptableInstallation : ScriptableObject,
        IInstallation
    {
        public abstract void Install(IContainer container);
    }
}
