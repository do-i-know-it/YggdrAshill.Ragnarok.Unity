#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace YggdrAshill.Ragnarok.Experimental
{
    public sealed class ImplicitMonoEntryPoint : MonoEntryPoint
    {
        protected override IEnumerable<IInstallation> InstallationList => DepthFirstInstallationList(transform);

        private IEnumerable<IInstallation> DepthFirstInstallationList(Transform target)
        {
            var current = target.GetComponents<IInstallation>().Where(candidate =>
                candidate is not ImplicitMonoEntryPoint entryPoint || entryPoint != this);
            
            var child = Enumerable.Range(0, target.childCount).Select(target.GetChild).SelectMany(child => 
            {
                if (child.TryGetComponent<Lifecycle>(out _))
                {
                    return Array.Empty<IInstallation>();
                }
                
                if (child.TryGetComponent<ImplicitMonoEntryPoint>(out var implicitMonoEntryPoint))
                {
                    return new[] { implicitMonoEntryPoint };
                }

                if (child.TryGetComponent<ExplicitMonoEntryPoint>(out var explicitMonoEntryPoint))
                {
                    return new[] { explicitMonoEntryPoint };
                }

                return DepthFirstInstallationList(child);
            });

            return current.Concat(child);
        }
    }
}
