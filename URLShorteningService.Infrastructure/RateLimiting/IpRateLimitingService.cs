using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URLShorteningService.Infrastructure.RateLimiting
{
    public class IpRateLimitingService
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger<IpRateLimitingService> _logger;
        private const int MaxRequestsPerMinute = 100;

        public IpRateLimitingService(IDistributedCache cache, ILogger<IpRateLimitingService> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public async Task<bool> IsAllowedAsync(string ipAddress)
        {
            try
            {
                var cacheKey = $"ratelimit_{ipAddress}";
                var countString = await _cache.GetStringAsync(cacheKey);
                int count = countString == null ? 0 : int.Parse(countString);

                if (count >= MaxRequestsPerMinute)
                    return false;

                count++;
                await _cache.SetStringAsync(cacheKey, count.ToString(), new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
                });

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking rate limit for IP {IpAddress}", ipAddress);
                return true; 
            }
        }
    }
}
