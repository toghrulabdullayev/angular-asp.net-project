namespace Server.Extensions
{
	public static class SwaggerExtensions
	{
		public static IServiceCollection AddSwaggerExplorer(this IServiceCollection services)
		{
			// registers metadata system for API to be used by Swagger (Swashbuckle lib, NOT USED after .NET 9.0)
			services.AddEndpointsApiExplorer();

			// registers Swagger generator (OpenAPI docs)
			services.AddSwaggerGen();
			return services;
		}

		public static WebApplication ConfigureSwaggerExplorer(this WebApplication app)
		{
			// adds middleware for generating API docs and serving Swagger UI in dev mode
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}
			return app;
		}
	}
}
