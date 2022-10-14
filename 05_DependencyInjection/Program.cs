using DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using static System.Formats.Asn1.AsnWriter;

///
/// Creates an IHostBuilder instance with the default binder settings.
/// Each services.Add{LIFETIME}<{SERVICE}> extension method adds (and potentially configures) services.
/// Configures services and adds them with their corresponding service lifetime.
/// Calls Build() and assigns an instance of IHost.
///
using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
        services.AddTransient<ITransientOperation, MyOperation>()
            .AddScoped<IScopedOperation, MyOperation>()
            .AddSingleton<ISingletonOperation, MyOperation>()
            .AddTransient<OperationLoggerSrv>())
    .Build();

///
///Calls ExemplifyScoping, passing in the IHost.Services.
///I added 3 identical call just to see how the corresponding service lifetime is working for consecutive Service calls 
///
ExemplifyScoping(host.Services, "Scope 1");
ExemplifyScoping(host.Services, "Scope 2");
ExemplifyScoping(host.Services, "Scope 3");

await host.RunAsync();


///
///Transient operations are always different, a new instance is created with every retrieval of the service.
///Scoped operations change only with a new scope, but are the same instance within a scope.
///Singleton operations are always the same, a new instance is only created once.
///
static void ExemplifyScoping(IServiceProvider services, string scope)
{
    using IServiceScope serviceScope = services.CreateScope();
    IServiceProvider provider = serviceScope.ServiceProvider;

    OperationLoggerSrv logger = provider.GetRequiredService<OperationLoggerSrv>();
    logger.LogOperations($"{scope}-Call 1 .GetRequiredService<OperationLoggerSrv>()");
    Console.WriteLine("...");

    logger = provider.GetRequiredService<OperationLoggerSrv>();
    logger.LogOperations($"{scope}-Call 2 .GetRequiredService<OperationLoggerSrv>()");
    Console.WriteLine();

    logger = provider.GetRequiredService<OperationLoggerSrv>();
    logger.LogOperations($"{scope}-Call 3 .GetRequiredService<OperationLoggerSrv>()");
    Console.WriteLine();
}