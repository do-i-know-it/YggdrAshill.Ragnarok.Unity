#nullable enable
using UnityEngine;

namespace YggdrAshill.Ragnarok
{
    public abstract class ScriptableEntryPoint : ScriptableObject,
        IEntryPoint
    {
        private IInstallation? installation;
        public IInstallation Installation
        {
            get
            {
                if (installation == null)
                {
                    installation = new Installation(Configure);
                }

                return installation;
            }
        }
        protected abstract void Configure(IContainer container);
    }
}
