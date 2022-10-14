using static System.Guid;

namespace DependencyInjection
{
    internal class MyOperation : ITransientOperation, IScopedOperation, ISingletonOperation
    {
        public string OperID { get; } = NewGuid().ToString()[^4..];//get 1st 4 chars of a GUID
    }
}
