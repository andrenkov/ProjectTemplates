using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjection
{
    /// <summary>
    /// Base Interface
    /// </summary>
    public interface IMyOperationInterface
    {
        string OperID { get; } 
    }

    public interface ITransientOperation : IMyOperationInterface
    {
    }

    public interface IScopedOperation : IMyOperationInterface
    {
    }

    public interface ISingletonOperation : IMyOperationInterface
    {
    }
}
