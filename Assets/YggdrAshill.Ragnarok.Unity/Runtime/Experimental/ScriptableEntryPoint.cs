#nullable enable
using System;
using UnityEngine;

namespace YggdrAshill.Ragnarok.Experimental
{
    [CreateAssetMenu(fileName = "ScriptableEntryPoint", menuName = "YggdrAshill.Ragnarok/EntryPoint")]
    public sealed class ScriptableEntryPoint : ScriptableInstallation
    {
        [SerializeField] private ScriptableInstallation[] installationList = Array.Empty<ScriptableInstallation>();

        public override void Install(IObjectContainer container)
        {
            foreach (var installation in installationList)
            {
                installation.Install(container);
            }
        }
    }
}
