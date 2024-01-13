#nullable enable
using System;
using UnityEngine;

namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class NoDependencyComponentInstallation : MonoInstallation
    {
        [SerializeField] private NoDependencyComponent? component;
        private NoDependencyComponent Component
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

        public override void Install(IObjectContainer container)
        {
            container.RegisterComponent(Component);
        }
    }
}
