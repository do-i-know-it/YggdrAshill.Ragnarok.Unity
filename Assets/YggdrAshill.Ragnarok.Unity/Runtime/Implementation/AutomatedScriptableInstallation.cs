#nullable enable
using System;
using UnityEngine;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    [CreateAssetMenu(fileName = "AutomatedScriptableInstallation", menuName = "YggdrAshill.Ragnarok/Installation")]
    public sealed class AutomatedScriptableInstallation : ScriptableInstallation
    {
        [SerializeField] private Automation[] automationList = Array.Empty<Automation>();

        public override void Install(IObjectContainer container)
        {
            foreach (var automation in automationList)
            {
                var statement = new CreateComponentInNewPrefabStatement(container.Compilation, Lifetime.Global, automation.Component);

                Automation.Register(statement, automation.InstanceInjectionTarget, automation.TypeAssignmentMethod);
                
                container.Registration.Register(statement);
            }
        }
    }
}
