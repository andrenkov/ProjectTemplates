using static System.Guid;

namespace DependencyInjection
{
    /// <summary>
    /// Class implementing 3 interfaces
    /// </summary>
    internal class MyOperation : ITransientOperation, IScopedOperation, ISingletonOperation
    {
        public string OperID { get; } = NewGuid().ToString()[^4..];//get 1st 4 chars of a GUID
    }
}
