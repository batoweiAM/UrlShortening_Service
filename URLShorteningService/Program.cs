
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using URLShorteningService.Application;
using URLShorteningService.Infrastructure;
using URLShorteningService.Infrastructure.Persistence;

namespace URLShorteningService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                var builder = WebApplication.CreateBuilder(args);

     
                Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(builder.Configuration)
                    .CreateLogger();

                builder.Host.UseSerilog();

             
                builder.Services.AddControllers();
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "URL Shortener API",
                        Version = "v1",
                        Description = "A simple URL shortening service API"
                    });
                });

               
                builder.Services.AddCors(options =>
                {
                    options.AddDefaultPolicy(builder =>
                    {
                        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                    });
                });

                builder.Services
                    .AddApplication()
                    .AddInfrastructure(builder.Configuration);

          
                builder.Services.AddHealthChecks()
                    .AddDbContextCheck<ApplicationDbContext>()
                    .AddRedis(builder.Configuration["RedisCacheSettings:ConnectionString"]!);
                builder.Services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                });

                var app = builder.Build();

                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();

                    using var scope = app.Services.CreateScope();
                    var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
                    await initialiser.InitialiseAsync();
                }

                app.UseSerilogRequestLogging();
                app.UseHttpsRedirection();

                app.UseCors();

                app.UseAuthorization();

                app.MapControllers();
                app.MapHealthChecks("/health");

                await app.RunAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
