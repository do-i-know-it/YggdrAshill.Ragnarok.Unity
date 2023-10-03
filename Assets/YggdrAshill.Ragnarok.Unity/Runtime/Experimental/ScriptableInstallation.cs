#nullable enable
using UnityEngine;

namespace YggdrAshill.Ragnarok.Experimental
{
    // TODO: add document comments.
    public abstract class ScriptableInstallation : ScriptableObject, IInstallation
    {
        public abstract void Install(IObjectContainer container);
    }
}
