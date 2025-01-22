using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URLShorteningService.Infrastructure.RateLimiting
{
    public class RateLimitingOptions
    {
        public int MaxRequestsPerMinute { get; set; } = 100;
        public string[] IpWhitelist { get; set; } = Array.Empty<string>();
    }
}
