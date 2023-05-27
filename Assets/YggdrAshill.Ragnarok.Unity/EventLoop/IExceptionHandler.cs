#nullable enable
using System;

namespace YggdrAshill.Ragnarok.Unity
{
    public interface IExceptionHandler
    {
        void Execute(Exception exception);
    }
}
