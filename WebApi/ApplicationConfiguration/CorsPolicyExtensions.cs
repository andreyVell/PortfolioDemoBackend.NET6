namespace WebApi.ApplicationConfiguration
{
    public static class CorsPolicyExtensions
    {
        private const string CorsPolicy = "AvetonTrialCorsPolicy";
        public static void AddCustomCorsPolicy(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: CorsPolicy,
                    builder => builder.WithOrigins(configuration.GetSection("AllowedOrigins").Get<List<string>>().ToArray())
                        .AllowCredentials()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
        }

        public static void UseCustomCorsPolicy(this IApplicationBuilder app)
        {
            app.UseCors(CorsPolicy);
        }
    }
}
