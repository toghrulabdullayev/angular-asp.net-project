using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Extensions
{
	public static class EFCoreExtensions
	{
		// AddDbContext already exists in services
		public static IServiceCollection InjectDbContext(
			this IServiceCollection services,
			IConfiguration config
		)
		{
			// DevDB is pulled from appsettings.json
			services.AddDbContext<AppDbContext>(options =>
				options.UseSqlServer(config.GetConnectionString("DevDB"))
			);
			return services;
		}
	}
}
