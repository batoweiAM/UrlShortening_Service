using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URLShorteningService.Infrastructure.Cache;
using URLShorteningService.Infrastructure.Persistence.Repositories;
using URLShorteningService.Infrastructure.Persistence;
using URLShorteningService.Infrastructure.RateLimiting;
using URLShorteningService.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using URLShorteningService.Application.Common.Interfaces;
using URLShorteningService.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace URLShorteningService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Database
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            // Repositories
            services.AddScoped<IUrlRepository, UrlRepository>();

            // Core services
            services.AddSingleton<IDateTime, DateTimeService>();

            // Redis Cache Configuration
            services.Configure<RedisCacheSettings>(
                configuration.GetSection("RedisCacheSettings"));

            services.AddStackExchangeRedisCache(options =>
            {
                var redisCacheSettings = configuration
                    .GetSection("RedisCacheSettings")
                    .Get<RedisCacheSettings>();

                options.Configuration = redisCacheSettings.ConnectionString;
                options.InstanceName = redisCacheSettings.InstanceName;
            });

            // Rate limiting configuration
            var rateLimitingSection = configuration.GetSection("RateLimiting");
            services.Configure<RateLimitingOptions>(rateLimitingSection);

            // Application services
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IpRateLimitingService>();

            // Logging
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddSerilog(new LoggerConfiguration()
                    .ReadFrom.Configuration(configuration)
                    .CreateLogger());
            });

            return services;
        }
    }
}
