using System;

namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class UnityEventLoop :
        IInitializable,
        IPreUpdatable,
        IPostUpdatable,
        IPreLateUpdatable,
        IPostLateUpdatable,
        IDisposable
    {
        public bool Initialized { get; private set; }
        public int PreUpdateExecutedCount { get; private set; }
        public int PostUpdateExecutedCount { get; private set; }
        public int PreLateUpdateExecutedCount { get; private set; }
        public int PostLateUpdateExecutedCount { get; private set; }
        public bool Disposed { get; private set; }
        
        public void Initialize()
        {
            Initialized = true;
        }

        public void PreUpdate()
        {
            PreUpdateExecutedCount++;
        }

        public void PostUpdate()
        {
            PostUpdateExecutedCount++;
        }

        public void PreLateUpdate()
        {
            PreLateUpdateExecutedCount++;
        }

        public void PostLateUpdate()
        {
            PostLateUpdateExecutedCount++;
        }

        public void Dispose()
        {
            Disposed = true;
        }
    }
}
