#nullable enable
namespace YggdrAshill.Ragnarok.Specification
{
    internal interface IResponseReceiver
    {
        void Receive(string response);
    }
}
