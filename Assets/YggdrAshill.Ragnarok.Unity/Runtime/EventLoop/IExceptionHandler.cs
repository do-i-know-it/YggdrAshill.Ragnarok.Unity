#nullable enable
using System;

namespace YggdrAshill.Ragnarok
{
    public interface IExceptionHandler
    {
        void Execute(Exception exception);
    }
}
