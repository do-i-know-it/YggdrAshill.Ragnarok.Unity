#nullable enable
using System;

namespace YggdrAshill.Ragnarok.Unity
{
    internal abstract class UnityEventLoopProcessor :
        IUnityEventLoopProcessor,
        IDisposable
    {
        private readonly UnityEventLoopExceptionHandler? exceptionHandler;

        protected UnityEventLoopProcessor(UnityEventLoopExceptionHandler? exceptionHandler)
        {
            this.exceptionHandler = exceptionHandler;
        }
        
        private bool disposed;

        public bool Process()
        {
            if (disposed) return false;

            try
            {
                Execute();
            }
            catch (Exception exception)
            {
                if (exceptionHandler == null)
                {
                    throw;
                }
                
                exceptionHandler.Invoke(exception);
                
                return false;
            }
            
            return !disposed;
        }

        protected abstract void Execute();
        
        public void Dispose()
        {
            disposed = true;
        }
    }
}
