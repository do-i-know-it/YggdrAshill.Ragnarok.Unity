#nullable enable
using YggdrAshill.Ragnarok.Construction;
using UnityEngine;

namespace YggdrAshill.Ragnarok.Unity
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
