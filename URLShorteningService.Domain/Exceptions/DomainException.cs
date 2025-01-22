using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URLShorteningService.Domain.Common;

namespace URLShorteningService.Domain.Exceptions
{
    public class DomainException : Exception
    {
        public Error Error { get; }

        public DomainException(Error error) : base(error.Message)
        {
            Error = error;
        }
    }
}
