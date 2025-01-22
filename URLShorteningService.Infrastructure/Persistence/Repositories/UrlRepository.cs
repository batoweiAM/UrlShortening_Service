using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URLShorteningService.Domain.Entities;
using URLShorteningService.Domain.Repositories;

namespace URLShorteningService.Infrastructure.Persistence.Repositories
{
    public class UrlRepository : IUrlRepository
    {
        private readonly ApplicationDbContext _context;

        public UrlRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UrlMapping?> GetByShortCodeAsync(string shortCode, CancellationToken cancellationToken = default)
        {
            return await _context.UrlMappings
                .Include(x => x.Stats)
                .FirstOrDefaultAsync(x => x.ShortCode == shortCode, cancellationToken);
        }

        public async Task<UrlMapping?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.UrlMappings
                .Include(x => x.Stats)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<bool> ExistsByShortCodeAsync(string shortCode, CancellationToken cancellationToken = default)
        {
            return await _context.UrlMappings
                .AnyAsync(x => x.ShortCode == shortCode, cancellationToken);
        }

        public async Task<UrlMapping> AddAsync(UrlMapping urlMapping, CancellationToken cancellationToken = default)
        {
            _context.UrlMappings.Add(urlMapping);
            await _context.SaveChangesAsync(cancellationToken);
            return urlMapping;
        }

        public async Task<UrlMapping> UpdateAsync(UrlMapping urlMapping, CancellationToken cancellationToken = default)
        {
            _context.Entry(urlMapping).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return urlMapping;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var urlMapping = await GetByIdAsync(id, cancellationToken);
            if (urlMapping == null) return false;

            _context.UrlMappings.Remove(urlMapping);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
