using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundQueueWorker
{
    public class MonitorLoop
    {
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly ILogger<MonitorLoop> _logger;
        private readonly CancellationToken _cancellationToken;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="taskQueue"></param>
        /// <param name="logger"></param>
        /// <param name="applicationLifetime"></param>
        public MonitorLoop(IBackgroundTaskQueue taskQueue, ILogger<MonitorLoop> logger,IHostApplicationLifetime applicationLifetime)
        {
            ///
            /// The IBackgroundTaskQueue is injected into the MonitorLoop service
            /// IBackgroundTaskQueue.QueueBackgroundWorkItemAsync is called to enqueue a work item
            /// The work item simulates a long-running background task
            ///
            _taskQueue = taskQueue;
            ///
            _logger = logger;
            _cancellationToken = applicationLifetime.ApplicationStopping;
        }

        public void StartMonitorLoop()
        {
            _logger.LogInformation($"{nameof(MonitorAsync)} loop is starting.");

            // Run a console user input loop in a background thread
            Task.Run(async () => await MonitorAsync());
        }

        /// <summary>
        /// Enqueuing tasks for the hosted service whenever the w key is selected on an input device
        /// </summary>
        /// <returns></returns>
        private async ValueTask MonitorAsync()
        {
            while (!_cancellationToken.IsCancellationRequested)
            {
                var keyStroke = Console.ReadKey();
                if (keyStroke.Key == ConsoleKey.W)
                {
                    // Enqueue a background work item
                    await _taskQueue.QueueBackgroundWorkItemAsync(BuildWorkItemAsync);
                }
            }
        }

        /// <summary>
        /// Simulate three 5-second tasks to complete for each enqueued work item
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private async ValueTask BuildWorkItemAsync(CancellationToken token)
        {
            int delayLoop = 0;
            var guid = Guid.NewGuid();

            _logger.LogInformation("Queued work item {Guid} is starting.", guid);

            while (!token.IsCancellationRequested && delayLoop < 3)
            {
                try
                {
                    //emulator of actual queue item processing 
                    await Task.Delay(TimeSpan.FromSeconds(5), token);
                }
                catch (OperationCanceledException)
                {
                    // Prevent throwing if the Delay is canceled
                }

                ++delayLoop;

                _logger.LogInformation("Queued work item {Guid} is running. {DelayLoop}/3", guid, delayLoop);
            }

            string format = delayLoop switch
            {
                3 => "Queued Background Task {Guid} is complete.",
                _ => "Queued Background Task {Guid} was cancelled."
            };

            _logger.LogInformation(format, guid);
        }
    }
}
