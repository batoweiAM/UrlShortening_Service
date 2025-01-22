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

            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IpRateLimitingService>();

            return services;
        }
    }
}
