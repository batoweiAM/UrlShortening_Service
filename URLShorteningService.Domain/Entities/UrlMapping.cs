using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URLShorteningService.Domain.Common;

namespace URLShorteningService.Domain.Entities
{
    public class UrlMapping : Entity
    {
        public string ShortCode { get; private set; }
        public string LongUrl { get; private set; }
        public DateTime? ExpiresAt { get; private set; }
        public bool IsActive { get; private set; }
        public virtual UrlStats Stats { get; private set; }

        private UrlMapping() { }

        private UrlMapping(string longUrl, string shortCode, DateTime? expiresAt)
        {
            LongUrl = longUrl;
            ShortCode = shortCode;
            ExpiresAt = expiresAt;
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
            Stats = new UrlStats();
        }

        public static Result<UrlMapping> Create(string longUrl, DateTime? expiresAt = null)
        {
            if (string.IsNullOrWhiteSpace(longUrl))
                return Result<UrlMapping>.Failure(DomainErrors.UrlMapping.EmptyUrl);

            if (!Uri.TryCreate(longUrl, UriKind.Absolute, out _))
                return Result<UrlMapping>.Failure(DomainErrors.UrlMapping.InvalidUrl);

            var shortCode = GenerateShortCode();

            return Result<UrlMapping>.Success(new UrlMapping(longUrl, shortCode, expiresAt));
        }

        private static string GenerateShortCode()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray())[..8]
                .Replace("/", "_")
                .Replace("+", "-");
        }

        public void IncrementAccessCount()
        {
            Stats.IncrementAccessCount();
            ModifiedAt = DateTime.UtcNow;
        }

        public void Deactivate()
        {
            IsActive = false;
            ModifiedAt = DateTime.UtcNow;
        }

        public bool IsExpired() => ExpiresAt.HasValue && ExpiresAt.Value < DateTime.UtcNow;
    }
}
