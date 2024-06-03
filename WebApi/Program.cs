using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Services.Helpers;
using Services.Interfaces;
using Services.SignalRHubs;
using Swashbuckle.AspNetCore.Filters;
using WebApi.ApplicationConfiguration;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            //register services autofac
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new ApiDiModule()));            
            //UTC converter with Z at the end
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    // Convert all dates to UTC
                    options.JsonSerializerOptions.Converters.Add(new UtcDateTimeConverter());
                });
            builder.Services.AddCustomValidators();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });
            //Custom request payload size 100MB
            builder.WebHost.ConfigureKestrel(serverOptions =>
            {
                serverOptions.Limits.MaxRequestBodySize = 101 * 1024 * 1024;
            });
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddCustomSqlContext(builder.Configuration);
            builder.Services.AddCustomJwtBearerAuthentication(builder.Configuration);            
            builder.Services.AddCustomCorsPolicy(builder.Configuration);
            builder.Services.AddCustomAutoMapper();
            

            builder.Services.AddSignalR();
            builder.Services.AddHttpClient();

            var app = builder.Build();
            var globalSettings = app.Services.GetRequiredService<IGlobalSettings>();
            Encryption.Initialize(globalSettings);
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCustomCorsPolicy();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapHub<ChatsHub>("/api/ChatsHub");
            app.MapHub<NotificationsHub>("/api/NotificationsHub");
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.Run();
        }
    }
}