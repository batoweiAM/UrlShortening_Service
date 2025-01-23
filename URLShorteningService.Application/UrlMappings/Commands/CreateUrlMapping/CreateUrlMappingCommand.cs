using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URLShorteningService.Application.UrlMappings.DTOs;
using URLShorteningService.Domain.Common;

namespace URLShorteningService.Application.UrlMappings.Commands.CreateUrlMapping
{
    public record CreateUrlMappingCommand(string LongUrl, DateTime? ExpiresAt) : IRequest<Result<UrlMappingDto>>;

    public class CreateUrlMappingCommandValidator : AbstractValidator<CreateUrlMappingCommand>
    {
        public CreateUrlMappingCommandValidator()
        {
            RuleFor(x => x.LongUrl)
                .NotEmpty()
                .MaximumLength(2048)
                .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
                .WithMessage("The URL must be valid and absolute");

            RuleFor(x => x.ExpiresAt)
                .Must(x => !x.HasValue || x.Value > DateTime.UtcNow)
                .WithMessage("Expiration date must be in the future");
        }
    }

}
