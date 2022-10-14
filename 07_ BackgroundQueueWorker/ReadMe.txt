https://learn.microsoft.com/en-us/dotnet/core/extensions/queue-service

1. Create new project from Worker Service template.
2. Add IBackgroundTaskQueue interface to the project to model a service. 
   There are two methods, one that exposes queuing functionality, and another that dequeues previously queued work items. 
   A work item is a Func<CancellationToken, ValueTask>. Next, add the default implementation to the project.
3. Add the default implementation to the project class DefaultBackgroundTaskQueue.cs. 
4. Rewrite the Worker class created by the VS Template by default. Worker class --> QueuedHostedService class.
5. Add MonitorLoop service to handle enqueuing tasks for the hosted service whenever the w key is selected on an input device.
6. Update IHost host = Host.CreateDefaultBuilder(args) in Program.cs to add services into the Host.
7. The service will start background processing when a user enters "w" in console. 