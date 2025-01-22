using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URLShorteningService.Domain.Entities
{
    public class UrlStats : Entity
    {
        public int UrlMappingId { get; private set; }
        public int AccessCount { get; private set; }
        public DateTime? LastAccessedAt { get; private set; }
        public virtual UrlMapping UrlMapping { get; private set; }

        public UrlStats()
        {
            AccessCount = 0;
            CreatedAt = DateTime.UtcNow;
        }

        public void IncrementAccessCount()
        {
            AccessCount++;
            LastAccessedAt = DateTime.UtcNow;
            ModifiedAt = DateTime.UtcNow;
        }
    }
}
