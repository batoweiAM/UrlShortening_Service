using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URLShorteningService.Application.Common.Interfaces;
using URLShorteningService.Application.UrlMappings.DTOs;

namespace URLShorteningService.Application.UrlMappings.Queries.GetUrlMapping
{
    public record GetUrlMappingQuery(string ShortCode) : IRequest<Result<UrlMappingDto>>;

    public class GetUrlMappingQueryHandler : IRequestHandler<GetUrlMappingQuery, Result<UrlMappingDto>>
    {
        private readonly IUrlRepository _urlRepository;
        private readonly ICacheService _cacheService;

        public GetUrlMappingQueryHandler(IUrlRepository urlRepository, ICacheService cacheService)
        {
            _urlRepository = urlRepository;
            _cacheService = cacheService;
        }

        public async Task<Result<UrlMappingDto>> Handle(GetUrlMappingQuery request, CancellationToken cancellationToken)
        {
            var cachedUrl = await _cacheService.GetAsync<string>($"url_{request.ShortCode}", cancellationToken);
            if (cachedUrl != null)
            {
                var urlMapping = await _urlRepository.GetByShortCodeAsync(request.ShortCode, cancellationToken);
                if (urlMapping != null)
                {
                    await _urlRepository.UpdateAsync(urlMapping, cancellationToken);
                    return Result.Success(new UrlMappingDto(
                        urlMapping.Id,
                        urlMapping.ShortCode,
                        urlMapping.LongUrl,
                        urlMapping.CreatedAt,
                        urlMapping.ExpiresAt,
                        urlMapping.IsActive));
                }
            }

            var mapping = await _urlRepository.GetByShortCodeAsync(request.ShortCode, cancellationToken);
            if (mapping == null)
                return Result.Failure<UrlMappingDto>(DomainErrors.UrlMapping.NotFound);

            if (mapping.IsExpired())
                return Result.Failure<UrlMappingDto>(DomainErrors.UrlMapping.Expired);

            if (!mapping.IsActive)
                return Result.Failure<UrlMappingDto>(DomainErrors.UrlMapping.Inactive);

            mapping.IncrementAccessCount();
            await _urlRepository.UpdateAsync(mapping, cancellationToken);

            await _cacheService.SetAsync($"url_{mapping.ShortCode}", mapping.LongUrl,
                TimeSpan.FromHours(24), cancellationToken);

            return Result.Success(new UrlMappingDto(
                mapping.Id,
                mapping.ShortCode,
                mapping.LongUrl,
                mapping.CreatedAt,
                mapping.ExpiresAt,
                mapping.IsActive));
        }
    }
}
