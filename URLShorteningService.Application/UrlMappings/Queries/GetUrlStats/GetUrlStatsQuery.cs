using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URLShorteningService.Application.UrlMappings.DTOs;
using URLShorteningService.Domain.Common;
using URLShorteningService.Domain.Repositories;

namespace URLShorteningService.Application.UrlMappings.Queries.GetUrlStats
{
    public record GetUrlStatsQuery(string ShortCode) : IRequest<Result<UrlStatsDto>>;

    public class GetUrlStatsQueryHandler : IRequestHandler<GetUrlStatsQuery, Result<UrlStatsDto>>
    {
        private readonly IUrlRepository _urlRepository;

        public GetUrlStatsQueryHandler(IUrlRepository urlRepository)
        {
            _urlRepository = urlRepository;
        }

        public async Task<Result<UrlStatsDto>> Handle(GetUrlStatsQuery request, CancellationToken cancellationToken)
        {
            var urlMapping = await _urlRepository.GetByShortCodeAsync(request.ShortCode, cancellationToken);

            if (urlMapping == null)
                return Result<UrlStatsDto>.Failure(DomainErrors.UrlMapping.NotFound);

            return Result<UrlStatsDto>.Success(new UrlStatsDto(
                urlMapping.Stats.AccessCount,
                urlMapping.Stats.LastAccessedAt,
                urlMapping.Stats.CreatedAt));
        }
    }

}
