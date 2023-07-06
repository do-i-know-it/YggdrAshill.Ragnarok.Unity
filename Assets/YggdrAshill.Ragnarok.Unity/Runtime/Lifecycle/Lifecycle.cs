#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace YggdrAshill.Ragnarok
{
    [DisallowMultipleComponent]
    public abstract class Lifecycle : MonoBehaviour,
        IDisposable
    {
        private IScope? scope;
        private IScope Scope
        {
            get
            {
                Build();

                return scope!;
            }
        }
        
        private IContext? currentContext;
        private IContext CurrentContext
        {
            get
            {
                if (currentContext == null)
                {
                    currentContext = GetCurrentContext();
                }

                return currentContext;
            }
        }
        protected abstract IContext GetCurrentContext();

        private IInstallation? installation;
        private IInstallation Installation
        {
            get
            {
                if (installation == null)
                {
                    var installationList
                        = GetEntryPointList().Select(entrypoint => entrypoint.Installation).ToArray();
                    installation = new Installation(installationList);
                }

                return installation;
            }
        }
        protected abstract IEnumerable<IEntryPoint> GetEntryPointList();

        private void Build()
        {
            if (scope != null)
            {
                return;
            }

            Installation.Install(CurrentContext);

            Configure(CurrentContext);

            scope = CurrentContext.Build();
        }
        private void Configure(IContainer container)
        {
            container.RegisterInstance(this).As<IDisposable>();
            container.UseUnityEventLoop();
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
