1. Add  Microsoft.Extensions.Hosting package
2. Add an interface(s). 
3. Set the lifetime "Transient" or "Singleton" or "Scoped".
4. Add a class(es) implementing it (or multiple inheritance).
5. Add service that requires DI (OperationLoggerSrv.cs). The constructor of the Service will requite the Interfaces above 
   (readonly properties). 
6. Add the exposed method LogOperations(string scope) to log the Operation with given Scope parameter.
7. Register the Service to DI. Require:
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;
8. The following is because the service implements three Interfaces as per its Constructor:

        services.AddTransient<ITransientOperation, DefaultOperation>()
            .AddScoped<IScopedOperation, DefaultOperation>()
            .AddSingleton<ISingletonOperation, DefaultOperation>()
            .AddTransient<OperationLogger>())