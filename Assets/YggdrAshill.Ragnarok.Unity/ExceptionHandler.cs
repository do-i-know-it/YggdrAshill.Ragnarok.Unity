#nullable enable
using System;

namespace YggdrAshill.Ragnarok.Unity
{
    internal sealed class ExceptionHandler :
        IExceptionHandler
    {
        private readonly Action<Exception> onExecuted;

        public ExceptionHandler(Action<Exception> onExecuted)
        {
            this.onExecuted = onExecuted;
        }
        
        public void Execute(Exception exception)
        {
            onExecuted.Invoke(exception);
        }
    }
}
