#nullable enable
using System;
using UnityEngine;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public sealed class AutomatedMonoInstallation : MonoInstallation
    {
        [SerializeField] private Automation[] automationList = Array.Empty<Automation>();
        
        public override void Install(IObjectContainer container)
        {
            foreach (var automation in automationList)
            {
                var statement = new ReturnComponentStatement(automation.Component, container.Compilation);

                Automation.Register(statement.InstanceInjection, automation.InstanceInjectionTarget, automation.TypeAssignmentMethod);
                
                container.Registration.Register(statement);
            }
        }
    }
}
