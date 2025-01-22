using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URLShorteningService.Application.UrlMappings.DTOs
{
    public record UrlMappingDto(
     int Id,
     string ShortCode,
     string LongUrl,
     DateTime CreatedAt,
     DateTime? ExpiresAt,
     bool IsActive);
}
