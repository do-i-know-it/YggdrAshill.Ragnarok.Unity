#nullable enable
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok.Experimental
{
    public abstract class MonoEntryPoint : MonoInstallation
    {
        protected abstract IEnumerable<IInstallation> InstallationList { get; }

        public sealed override void Install(IContainer container)
        {
            foreach (var installation in InstallationList)
            {
                installation.Install(container);
            }
        }
    }
}
