#nullable enable
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok.Experimental
{
    public abstract class ScriptableEntryPoint : ScriptableInstallation
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
