using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Server.Extensions;
using Server.Models; //! Note: some imports are not implicit

// creates DI container, config system, logging, hosting environment and etc
var builder = WebApplication.CreateBuilder(args);

//* makes them available

// registers controllers, MVC system
builder.Services.AddControllers();

// extension methods created in Extensions directory
builder
	.Services.AddSwaggerExplorer()
	.InjectDbContext(builder.Configuration)
	.AddIdentityHandlersAndStores()
	.ConfigureIdentityOptions()
	.AddIdentityAuth(builder.Configuration);

// creates the actual application
var app = builder.Build();

//* activates their behavior in pipeline

app.ConfigureSwaggerExplorer().ConfigureCORS(builder.Configuration).AddIdentityAuthMiddlewares();

// maps routes to controllers
app.MapControllers();

// maps respective routes (auth related endpoints) and groups into /api
app.MapGroup("/api").MapIdentityApi<AppUser>();

app.MapPost(
	"/api/signup",
	async (
		UserManager<AppUser> userManager,
		[FromBody] UserRegistrationModel userRegistrationModel
	) =>
	{
		//? Object Initializer, equivalent of user.Email=...;user.FullName=...;
		AppUser user = new AppUser()
		{
			UserName = userRegistrationModel.Email, // username is required in Identity, and this is the only way to bypass it
			Email = userRegistrationModel.Email,
			FullName = userRegistrationModel.FullName,
		};

		//* Password is passed here because it gets hashed, unlike if you passed it to new AppUser()
		var result = await userManager.CreateAsync(user, userRegistrationModel.Password!);
		if (result.Succeeded)
			return Results.Ok(result);
		else
			return Results.BadRequest(result);
	}
);

// starts the server and listens for HTTP requests
app.Run();

public class UserRegistrationModel
{
	public string? Email { get; set; }
	public string? Password { get; set; }
	public string? FullName { get; set; }
}
