using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Server.Models;

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
  .AddIdentityApiEndpoints<IdentityUser>()
  .AddEntityFrameworkStores<AppDbContext>();

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

// starts the server and listens for HTTP requests
app.Run();
