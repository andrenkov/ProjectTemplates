using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundQueueWorker
{
    /// <summary>
    /// There are two methods, one that exposes queuing functionality, and another that dequeues previously queued work items. 
    /// A work item is a Func<CancellationToken, ValueTask>.
    /// </summary>
    public interface IBackgroundTaskQueue
    {
        ValueTask QueueBackgroundWorkItemAsync(Func<CancellationToken, ValueTask> workItem);

        ValueTask<Func<CancellationToken, ValueTask>> DequeueAsync(CancellationToken cancellationToken);
    }
}
