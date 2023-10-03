#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    [DisallowMultipleComponent]
    public abstract class Lifecycle : MonoBehaviour, IDisposable
    {
        private IObjectScope? scope;
        private IObjectScope Scope
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

        public IObjectResolver Resolver => Scope.Resolver;

        private IObjectScope BuildInternally()
        {
            var context = GetCurrentContext();

            foreach (var installation in GetInstallationList())
            {
                installation.Install(context);
            }
            
            context.RegisterInstance(this).AsOwnSelf();
            context.UseUnityEventLoop();

            return context.CreateScope();
        }
        
        protected abstract IObjectContext GetCurrentContext();
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

        public IObjectContext CreateChildContext()
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
