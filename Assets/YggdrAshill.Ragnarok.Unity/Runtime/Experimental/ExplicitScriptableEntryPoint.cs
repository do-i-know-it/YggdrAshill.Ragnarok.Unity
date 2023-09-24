#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;

namespace YggdrAshill.Ragnarok.Experimental
{
    [CreateAssetMenu(fileName = "ExplicitScriptableEntryPoint", menuName = "YggdrAshill.Ragnarok/EntryPoint")]
    public sealed class ExplicitScriptableEntryPoint : ScriptableEntryPoint
    {
        [SerializeField] private ScriptableInstallation[] scriptableInstallationList
            = Array.Empty<ScriptableInstallation>();
        protected override IEnumerable<IInstallation> InstallationList => scriptableInstallationList;
    }
}
