using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URLShorteningService.Domain.Common
{
    public static class DomainErrors
    {
        public static class UrlMapping
        {
            public static Error EmptyUrl => new("UrlMapping.EmptyUrl", "The URL cannot be empty.");
            public static Error InvalidUrl => new("UrlMapping.InvalidUrl", "The URL format is invalid.");
            public static Error NotFound => new("UrlMapping.NotFound", "The URL mapping was not found.");
            public static Error Expired => new("UrlMapping.Expired", "The URL mapping has expired.");
            public static Error Inactive => new("UrlMapping.Inactive", "The URL mapping is inactive.");
        }
    }
}
