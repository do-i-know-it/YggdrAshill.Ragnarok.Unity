#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;

namespace YggdrAshill.Ragnarok
{
    [DisallowMultipleComponent]
    public abstract class Lifecycle : MonoBehaviour,
        IDisposable
    {
        private IScope? scope;
        // TODO: Make this method public if needed.
        private IScope Scope
        {
            get
            {
                if (scope == null)
                {
                    scope = BuildInternally();
                }

                return scope;
            }
        }

        private IScope BuildInternally()
        {
            var context = GetCurrentContext();

            foreach (var installation in GetInstallationList())
            {
                installation.Install(context);
            }
            
            context.RegisterInstance(this).As<IDisposable>();
            context.UseUnityEventLoop();

            return context.Build();
        }
        
        protected abstract IContext GetCurrentContext();
        protected abstract IEnumerable<IInstallation> GetInstallationList();
        
        // TODO: Make this method public if needed.
        private void Build()
        {
            if (scope != null)
            {
                return;
            }

            scope = BuildInternally();
        }

        public IContext CreateChildContext()
        {
            return Scope.CreateContext();
        }

        public void Dispose()
        {
            if (this == null)
            {
                return;
            }

            // TODO: Destroy(this)?
            Destroy(gameObject);
        }

        protected abstract bool RunAutomatically { get; }

        protected virtual void Awake()
        {
            if (!RunAutomatically)
            {
                return;
            }

            Build();
        }

        protected virtual void OnDestroy()
        {
            if (scope == null)
            {
                return;
            }

            scope.Dispose();
            scope = null;
        }
    }
}
