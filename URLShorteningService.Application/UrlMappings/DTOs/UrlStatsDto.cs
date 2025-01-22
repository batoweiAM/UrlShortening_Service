using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URLShorteningService.Application.UrlMappings.DTOs
{
    public record UrlStatsDto(
    int AccessCount,
    DateTime? LastAccessedAt,
    DateTime CreatedAt);
}
