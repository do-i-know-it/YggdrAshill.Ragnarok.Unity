#nullable enable
using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok.Unity
{
    internal sealed class UnityEventLoopExceptionHandler
    {
        private readonly IReadOnlyList<IExceptionHandler> exceptionHandlerList;

        public UnityEventLoopExceptionHandler(IReadOnlyList<IExceptionHandler> exceptionHandlerList)
        {
            this.exceptionHandlerList = exceptionHandlerList;
        }

        public void Invoke(Exception exception)
        {
            foreach (var handler in exceptionHandlerList)
            {
                handler.Execute(exception);
            }
        }
    }
}
