using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URLShorteningService.Application.UrlMappings.DTOs;

namespace URLShorteningService.Application.UrlMappings.Commands.CreateUrlMapping
{
    public class CreateUrlMappingCommandHandler : IRequestHandler<CreateUrlMappingCommand, Result<UrlMappingDto>>
    {
        private readonly IUrlRepository _urlRepository;
        private readonly ICacheService _cacheService;

        public CreateUrlMappingCommandHandler(IUrlRepository urlRepository, ICacheService cacheService)
        {
            _urlRepository = urlRepository;
            _cacheService = cacheService;
        }

        public async Task<Result<UrlMappingDto>> Handle(CreateUrlMappingCommand request, CancellationToken cancellationToken)
        {
            var urlMappingResult = UrlMapping.Create(request.LongUrl, request.ExpiresAt);
            if (urlMappingResult.IsFailure)
                return Result.Failure<UrlMappingDto>(urlMappingResult.Error);

            var urlMapping = await _urlRepository.AddAsync(urlMappingResult.Value, cancellationToken);
            await _cacheService.SetAsync($"url_{urlMapping.ShortCode}", urlMapping.LongUrl,
                TimeSpan.FromHours(24), cancellationToken);

            return Result.Success(new UrlMappingDto(
                urlMapping.Id,
                urlMapping.ShortCode,
                urlMapping.LongUrl,
                urlMapping.CreatedAt,
                urlMapping.ExpiresAt,
                urlMapping.IsActive));
        }
    }
}
