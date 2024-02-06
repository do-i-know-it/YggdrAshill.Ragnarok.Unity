#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public sealed class ExplicitMonoEntryPoint : MonoEntryPoint
    {
        [SerializeField] private ScriptableInstallation[] scriptableInstallationList
            = Array.Empty<ScriptableInstallation>();
        private IEnumerable<IInstallation> ScriptableInstallationList => scriptableInstallationList;
        
        [SerializeField] private MonoInstallation[] monoInstallationList = Array.Empty<MonoInstallation>();
        private IEnumerable<IInstallation> MonoInstallationList => monoInstallationList;

        protected override IEnumerable<IInstallation> InstallationList
            => ScriptableInstallationList.Concat(MonoInstallationList);
    }
}
