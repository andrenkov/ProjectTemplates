https://learn.microsoft.com/en-ca/training/modules/build-web-api-aspnet-core/3-exercise-create-web-api

1. Create new Web Api project
2. Optional: install HttpRepl tool for use the .NET CLI for testing:
	dotnet tool install -g Microsoft.dotnet-httprepl
	For example, run 
		httprepl https://localhost:7009 
	to connect to the base of the Host
	then 
		https://localhost:7009/> get weatherforecast
	to call the endpoint
	You can also call "cd" to navigate to the endpoint and then call GET:
		https://localhost:7009/> cd weatherforecast
		/weatherforecast    [GET]
	"ls" command lists all available endpoints
3. Create new Model (class) in Models folder
4. Create new Service (class) in Services folder with List<> field for in-memory Db, constructor and methods (add, delete ...). 
5. Create new CactusController with actions colling the Service methods. 
