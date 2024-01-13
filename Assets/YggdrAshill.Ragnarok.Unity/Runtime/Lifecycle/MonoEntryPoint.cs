#nullable enable
using System.Collections.Generic;
using UnityEngine;

namespace YggdrAshill.Ragnarok
{
    [DisallowMultipleComponent]
    public abstract class MonoEntryPoint : MonoInstallation
    {
        protected abstract IEnumerable<IInstallation> InstallationList { get; }

        public sealed override void Install(IObjectContainer container)
        {
            foreach (var installation in InstallationList)
            {
                installation.Install(container);
            }
        }
    }
}
