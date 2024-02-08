#nullable enable
using System;
using UnityEngine;

namespace YggdrAshill.Ragnarok
{
    [Serializable]
    internal sealed class Automation
    {
        public static void Register(IObjectContainer container, Automation automation)
        {
            var statement = new ReturnComponentStatement(automation.Component, container.Compilation);
            container.Registration.Register(statement);
            
            Register(statement.InstanceInjection, automation.InstanceInjectionTarget, automation.TypeAssignmentMethod);

            var instruction = new ResolveWithStatement(statement);
            container.Registration.Register(instruction);
        }
        
        private static void Register(IInstanceInjection instanceInjection, InstanceInjectionTarget target, TypeAssignmentMethod method)
        {
            if ((target & InstanceInjectionTarget.Field) is InstanceInjectionTarget.Field)
            {
                instanceInjection.WithFieldInjection();
            }
            if ((target & InstanceInjectionTarget.Property) is InstanceInjectionTarget.Property)
            {
                instanceInjection.WithPropertyInjection();
            }
            if ((target & InstanceInjectionTarget.Method) is InstanceInjectionTarget.Method)
            {
                instanceInjection.WithMethodInjection();
            }
            
            switch (method)
            {
                case TypeAssignmentMethod.Self:
                    instanceInjection.AsOwnSelf();
                    break;
                case TypeAssignmentMethod.AllInterfaces:
                    instanceInjection.AsImplementedInterfaces();
                    break;
                case TypeAssignmentMethod.AllInterfacesAndSelf:
                    instanceInjection.AsImplementedInterfaces().AsOwnSelf();
                    break;
                default:
                    throw new InvalidOperationException($"{method} is invalid.");
            }
        }
        
        [SerializeField] private Component? component;
        public Component Component
        {
            get
            {
                if (component == null)
                {
                    throw new InvalidOperationException($"{nameof(component)} is null.");
                }

                return component;
            }
        }

        [SerializeField] private TypeAssignmentMethod typeAssignmentMethod = TypeAssignmentMethod.Self;
        public TypeAssignmentMethod TypeAssignmentMethod => typeAssignmentMethod;
        
        [SerializeField] private InstanceInjectionTarget instanceInjectionTarget = InstanceInjectionTarget.None;
        public InstanceInjectionTarget InstanceInjectionTarget => instanceInjectionTarget;
    }
}
