using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

builder.Services.AddAuthentication(x =>
{
  // chained assignment (x.value1 = x.value2 = x.value3 = ...). Don't get confused by newlines
  x.DefaultAuthenticateScheme =
  x.DefaultChallengeScheme =
  x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(y =>
{
  y.SaveToken = false;
  y.TokenValidationParameters = new TokenValidationParameters
  {
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(
      Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:JWTSecret"]!)
    ),
  };
});

// creates the actual application
var app = builder.Build();

//* activates their behavior in pipeline

// adds middleware for generating API docs and serving Swagger UI in dev mode
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

// region and endregion makes this part of the code collapsible, no other functional effects
#region Config. CORS
app.UseCors(options => options.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader());
#endregion

app.UseAuthentication();

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
