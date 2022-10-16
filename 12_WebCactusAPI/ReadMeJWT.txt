##############################################################################################
API
##############################################################################################
https://www.infoworld.com/article/3669188/how-to-implement-jwt-authentication-in-aspnet-core-6.html
https://dotnettutorials.net/lesson/token-based-authentication-web-api/
https://www.c-sharpcorner.com/article/how-to-use-jwt-authentication-with-web-api/

https://dotnetdetail.net/asp-net-core-5-web-api-token-based-authentication-example-using-jwt/
https://www.c-sharpcorner.com/article/authentication-and-authorization-in-asp-net-5-with-jwt-and-swagger/

https://codingsonata.com/apply-jwt-access-tokens-and-refresh-tokens-in-asp-net-core-web-api-6/

1. Create new API project
2. Install:
	Microsoft.AspNetCore.Authentication
	Microsoft.AspNetCore.Authentication.JwtBearer
3. Add Account Controller with Login endpoint and GenerateJwtToken method.
4. In “WeatherForcastController.cs” above Get() method, add this line 
   [Authorize(Roles = "Role1")]. Roles are used in the GenerateJwtToken(). 
5. Add options.AddSecurityDefinition into the Swagger config in Program.cs to see the Authorization option in th eSwagger UI
6. To test: 
	Call the Account.Loging and enter the body as:
	{
	  "username": "admin",
	  "password": "Admin@123"
	}
	Copy the token returned. 

	Click on Authorize and enter "Bearer mynewtoken"
	Then goto weatherforecast and exec Get(). 







##############################################################################################
MVC
##############################################################################################
https://www.codemag.com/Article/2105051/Implementing-JWT-Authentication-in-ASP.NET-Core-5

1. Create new Web MVC (!!!) project. Code examples are for MVC!!! with Views etc. See Home Controller for Views.
2. Install:
	Microsoft.AspNetCore.Authentication
	Microsoft.AspNetCore.Authentication.JwtBearer
3. Add HomeController.
4. Add ITokenService: This is an interface that contains the declaration of two methods, i.e., BuildToken and IsTokenValid.
5. Add class TokenService implementing the interface above.
6. Add IUserRepository interface This interface contains the declaration of the GetUser method 
   that's used to get an UserDTO instance based on the username from an instance of UserModel class.
7. Add UserRepository: The UserRepository class extends the IUserRepository interface and implements the GetUser method. 
   It also contains the sample data used by the application as a list of the UserDTO class.
8. Add UserModel class.
9. Configure JWT in the AppSettings File.
10.In Program.cs, add ConfigureServices method of the Startup class, I should mention that you'll use the 
   AddAuthentication feature as well as JwtBearer using the AddJwtBearer method.
11.In Program.cs, add Jwt intialization and add a transient service of type IUserRepository and IITokenService respectively.
12.Add DI into HomeController to inject IUserRepository and ITokenService.
13.To take advantage of session state, add storing/retrieving the generated token from the Session. 
   You should make a call to the UseSession() extension method in the Program.cs --> app.Use().
14. The tutorial also adds app.UseRouting() and app.UseEndpoints(). Not sure is this required indeed. 