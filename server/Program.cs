using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Models; //! Note: some imports are not implicit

// creates DI container, config system, logging, hosting environment and etc
var builder = WebApplication.CreateBuilder(args);

//* makes them available

// registers controllers, MVC system
builder.Services.AddControllers();

// registers metadata system for API to be used by Swagger (Swashbuckle lib, NOT USED after .NET 9.0)
builder.Services.AddEndpointsApiExplorer();

// registers Swagger generator (OpenAPI docs)
builder.Services.AddSwaggerGen();

// registers identity services (auth, entities, and etc. Refer to Identity architecture docs)
builder.Services
  .AddIdentityApiEndpoints<AppUser>()
  .AddEntityFrameworkStores<AppDbContext>();

// changes default configs in Identity package
builder.Services.Configure<IdentityOptions>(options =>
{
  options.Password.RequireDigit = false;
  options.Password.RequireUppercase = false;
  options.Password.RequireLowercase = false;
  options.User.RequireUniqueEmail = true;
});

// DevDB is pulled from appsettings.json
builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DevDB")));

// creates the actual application
var app = builder.Build();

//* activates their behavior in pipeline

// adds middleware for generating API docs and serving Swagger UI in dev mode
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

// checks permissions and runs after authentication if exists
app.UseAuthorization();

// maps routes to controllers
app.MapControllers();

// maps respective routes (auth related endpoints) and groups into /api
app
  .MapGroup("/api")
  .MapIdentityApi<AppUser>();

app.MapPost("/api/signup", async (
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
});

// starts the server and listens for HTTP requests
app.Run();

public class UserRegistrationModel
{
  public string? Email { get; set; }
  public string? Password { get; set; }
  public string? FullName { get; set; }
}
