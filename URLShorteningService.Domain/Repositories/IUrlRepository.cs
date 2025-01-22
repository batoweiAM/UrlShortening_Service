using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URLShorteningService.Domain.Entities;

namespace URLShorteningService.Domain.Repositories
{
    public interface IUrlRepository
    {
        Task<UrlMapping?> GetByShortCodeAsync(string shortCode, CancellationToken cancellationToken = default);
        Task<UrlMapping?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> ExistsByShortCodeAsync(string shortCode, CancellationToken cancellationToken = default);
        Task<UrlMapping> AddAsync(UrlMapping urlMapping, CancellationToken cancellationToken = default);
        Task<UrlMapping> UpdateAsync(UrlMapping urlMapping, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
