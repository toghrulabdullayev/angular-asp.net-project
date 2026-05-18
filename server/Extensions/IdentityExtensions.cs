using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Server.Models;

namespace Server.Extensions
{
	public static class IdentityExtensions
	{
		public static IServiceCollection AddIdentityHandlersAndStores(this IServiceCollection services)
		{
			// registers identity services (auth, entities, and etc. Refer to Identity architecture docs)
			services.AddIdentityApiEndpoints<AppUser>().AddEntityFrameworkStores<AppDbContext>();
			return services;
		}

		public static IServiceCollection ConfigureIdentityOptions(this IServiceCollection services)
		{ // changes default configs in Identity package
			services.Configure<IdentityOptions>(options =>
			{
				options.Password.RequireDigit = false;
				options.Password.RequireUppercase = false;
				options.Password.RequireLowercase = false;
				options.User.RequireUniqueEmail = true;
			});
			return services;
		}

		public static IServiceCollection AddIdentityAuth(
			this IServiceCollection services,
			IConfiguration config
		)
		{
			services
				.AddAuthentication(x =>
				{
					// chained assignment (x.value1 = x.value2 = x.value3 = ...). Don't get confused by newlines
					x.DefaultAuthenticateScheme =
						x.DefaultChallengeScheme =
						x.DefaultScheme =
							JwtBearerDefaults.AuthenticationScheme;
				})
				.AddJwtBearer(y =>
				{
					y.SaveToken = false;
					y.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = new SymmetricSecurityKey(
							Encoding.UTF8.GetBytes(config["AppSettings:JWTSecret"]!)
						),
					};
				});
			return services;
		}

		public static WebApplication AddIdentityAuthMiddlewares(this WebApplication app)
		{
			app.UseAuthentication();

			// checks permissions and runs after authentication if exists
			app.UseAuthorization();
			return app;
		}
	}
}
