using System.Threading.Channels;


namespace BackgroundQueueWorker
{
    /// <summary>
    /// This class implements the interface
    /// </summary>
    public class DefaultBackgroundTaskQueue : IBackgroundTaskQueue
    {
        /// <summary>
        /// work item is a Func<CancellationToken, ValueTask>.
        /// Channel<T> is a queue
        /// </summary>
        private readonly Channel<Func<CancellationToken, ValueTask>> _queue;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="capacity"></param>
        public DefaultBackgroundTaskQueue(int capacity)
        {
            ///
            /// BoundedChannelOptions(Int32) is called with an explicit capacity. 
            /// Capacity should be set based on the expected application load and number of concurrent threads accessing the queue. 
            ///
            BoundedChannelOptions options = new(capacity)
            {
                ///
                /// BoundedChannelFullMode.Wait will cause calls to ChannelWriter<T>.WriteAsync to return a task, 
                /// which completes only when space becomes available
                ///
                FullMode = BoundedChannelFullMode.Wait
            };
            _queue = Channel.CreateBounded<Func<CancellationToken, ValueTask>>(options);
        }

        /// <summary>
        /// Add Task to queue
        /// </summary>
        /// <param name="workItem"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async ValueTask QueueBackgroundWorkItemAsync(Func<CancellationToken, ValueTask> workItem)
        {
            if (workItem is null)
            {
                throw new ArgumentNullException(nameof(workItem));
            }

            await _queue.Writer.WriteAsync(workItem);
        }

        /// <summary>
        /// Read item from queue
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async ValueTask<Func<CancellationToken, ValueTask>> DequeueAsync(CancellationToken cancellationToken)
        {
            Func<CancellationToken, ValueTask>? workItem = await _queue.Reader.ReadAsync(cancellationToken);

            return workItem;
        }

    }
}
