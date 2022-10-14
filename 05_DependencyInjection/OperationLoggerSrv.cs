
namespace DependencyInjection
{
    public class OperationLoggerSrv
    {
        private readonly ITransientOperation _transientOperation;
        private readonly IScopedOperation _scopedOperation;
        private readonly ISingletonOperation _singletonOperation;

        /// <summary>
        /// Constructor with ID
        /// </summary>
        /// <param name="transientOperation"></param>
        /// <param name="scopedOperation"></param>
        /// <param name="singletonOperation"></param>
        public OperationLoggerSrv(
                ITransientOperation transientOperation,
                IScopedOperation scopedOperation,
                ISingletonOperation singletonOperation) =>
                (_transientOperation, _scopedOperation, _singletonOperation) =
                    (transientOperation, scopedOperation, singletonOperation);
        /// <summary>
        /// The exposed method to log the Operation with given Scope parameter
        /// </summary>
        /// <param name="scope"></param>
        public void LogOperations(string scope)
        {
            LogOperation(_transientOperation, scope, "Always different");
            LogOperation(_scopedOperation, scope, "Changes only with scope");
            LogOperation(_singletonOperation, scope, "Always the same");
        }

        private static void LogOperation<T>(T operation, string scope, string message)
             where T : IMyOperationInterface =>
             Console.WriteLine(
                 $"{scope}: {typeof(T).Name,-19} [ {operation.OperID}...{message,-23} ]");
    }
}
