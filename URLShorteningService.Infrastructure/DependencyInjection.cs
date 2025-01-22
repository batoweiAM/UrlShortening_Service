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
using Serilog;

namespace URLShorteningService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddScoped<ApplicationDbContextInitialiser>();

            services.AddScoped<IUrlRepository, UrlRepository>();

            services.AddSingleton<IDateTime, DateTimeService>();

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

            var rateLimitingSection = configuration.GetSection("RateLimiting");
            services.Configure<RateLimitingOptions>(rateLimitingSection);

            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IpRateLimitingService>();

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
