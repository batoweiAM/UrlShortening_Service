using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URLShorteningService.Domain.Entities
{
    public class Entity
    {
        public int Id { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public string? CreatedBy { get; protected set; }
        public DateTime? ModifiedAt { get; protected set; }
        public string? ModifiedBy { get; protected set; }
    }
}
