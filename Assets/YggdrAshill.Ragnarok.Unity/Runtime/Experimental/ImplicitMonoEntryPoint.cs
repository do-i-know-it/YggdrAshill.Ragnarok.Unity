#nullable enable
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace YggdrAshill.Ragnarok.Experimental
{
    [DisallowMultipleComponent]
    public sealed class ImplicitMonoEntryPoint : MonoEntryPoint
    {
        protected override IEnumerable<IInstallation> InstallationList => GetInstallationList(transform);
        private static IEnumerable<IInstallation> GetInstallationList(Transform target)
        {
            var current = GetCurrentInstallationList(target);

            var children = GetChildInstallationList(target);

            var grandChildren = GetGrandChildInstallationList(target);

            return current.Concat(children).Concat(grandChildren);
        }
        private static IEnumerable<IInstallation> GetCurrentInstallationList(Transform target)
        {
            return target.GetComponents<IInstallation>().Where(installation =>
            {
                var implicitMonoEntryPoint = installation as ImplicitMonoEntryPoint;
                var explicitMonoEntryPoint = installation as ExplicitMonoEntryPoint;

                return implicitMonoEntryPoint == null && explicitMonoEntryPoint == null;
            });
        }
        private static IEnumerable<IInstallation> GetChildInstallationList(Transform target)
        {
            return GetChildCandidate(target).SelectMany(GetCurrentInstallationList);
        }
        private static IEnumerable<IInstallation> GetGrandChildInstallationList(Transform target)
        {
            return GetChildCandidate(target).SelectMany(GetChildInstallationList);
        }

        private static IEnumerable<Transform> GetChildCandidate(Transform target)
        {
            return Enumerable.Range(0, target.childCount).Select(target.GetChild).Where(child =>
            {
                if (child.TryGetComponent<Lifecycle>(out _))
                {
                    return false;
                }

                if (child.TryGetComponent<ImplicitMonoEntryPoint>(out _))
                {
                    return false;
                }

                return !child.TryGetComponent<ExplicitMonoEntryPoint>(out _);
            });
        }
    }
}
