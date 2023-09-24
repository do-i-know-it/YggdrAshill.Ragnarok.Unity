#nullable enable
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace YggdrAshill.Ragnarok.Experimental
{
    public sealed class ImplicitMonoEntryPoint : MonoEntryPoint
    {
        protected override IEnumerable<IInstallation> InstallationList => GetInstallationList(transform);

        private IEnumerable<IInstallation> GetInstallationList(Transform target)
        {
            return Enumerable.Range(0, target.childCount)
                .Select(target.GetChild)
                .Where(child => !child.TryGetComponent<Lifecycle>(out _) || !child.TryGetComponent<ImplicitMonoEntryPoint>(out _))
                .SelectMany(GetInstallationList);
        }
    }
}
