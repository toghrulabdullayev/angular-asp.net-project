namespace Server.Extensions
{
	public static class AppConfigExtensions
	{
		public static WebApplication ConfigureCORS(this WebApplication app, IConfiguration config)
		{
			// region and endregion makes this part of the code collapsible, no other functional effects
			#region Config. CORS
			app.UseCors(options =>
				options.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader()
			);
			#endregion
			return app;
		}
	}
}
