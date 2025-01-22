using MediatR;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using URLShorteningService.Application.UrlMappings.Commands.CreateUrlMapping;
using URLShorteningService.Application.UrlMappings.Queries.GetUrlMapping;
using URLShorteningService.Application.UrlMappings.Queries.GetUrlStats;
using URLShorteningService.Infrastructure.RateLimiting;

namespace URLShorteningService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UrlController : ApiController
    {
        private readonly ISender _mediator;
        private readonly IpRateLimitingService _rateLimitingService;

        public UrlController(ISender mediator, IpRateLimitingService rateLimitingService)
        {
            _mediator = mediator;
            _rateLimitingService = rateLimitingService;
        }

        [HttpPost("shorten")]
        public async Task<IActionResult> CreateShortUrl(
            [FromBody] CreateUrlMappingCommand command,
            CancellationToken cancellationToken)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            if (!await _rateLimitingService.IsAllowedAsync(ipAddress))
                return StatusCode(StatusCodes.Status429TooManyRequests);

            var result = await _mediator.Send(command, cancellationToken);

            return result.IsSuccess
                ? CreatedAtAction(nameof(GetUrl), new { shortCode = result.Value.ShortCode }, result.Value)
                : Problem(result.Error);
        }

        [HttpGet("{shortCode}")]
        public async Task<IActionResult> GetUrl(
            string shortCode,
            CancellationToken cancellationToken)
        {
            var query = new GetUrlMappingQuery(shortCode);
            var result = await _mediator.Send(query, cancellationToken);

            if (result.IsFailure)
                return Problem(result.Error);

            return Redirect(result.Value.LongUrl);
        }

        [HttpGet("stats/{shortCode}")]
        public async Task<IActionResult> GetUrlStats(
            string shortCode,
            CancellationToken cancellationToken)
        {
            var query = new GetUrlStatsQuery(shortCode);
            var result = await _mediator.Send(query, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : Problem(result.Error);
        }
    }

}
