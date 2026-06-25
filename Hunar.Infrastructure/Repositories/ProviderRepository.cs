using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hunar.Domain.Entities;
using Hunar.Domain.Interfaces;
using Hunar.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hunar.Infrastructure.Repositories
{
    public class ProviderRepository : IProviderRepository
    {
        private readonly HunarDbContext _context;

        public ProviderRepository(HunarDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProviderProfile>> GetAllAsync()
        {
            // Include() tells EF to also load related data in the same query
            // Without Include, User and ServiceListings would be null
            // This generates a SQL JOIN automatically
            return await _context.ProviderProfiles
                .Include(p => p.User)
                .Include(p => p.ServiceListings)
                .ToListAsync();
        }

        public async Task<List<ProviderProfile>> GetByCategoryAsync(string category)
        {
            // LINQ Where clause — EF translates this to SQL WHERE
            return await _context.ProviderProfiles
                .Include(p => p.User)
                .Include(p => p.ServiceListings)
                .Where(p => p.Category.ToLower() == category.ToLower())
                .ToListAsync();
        }

        public async Task<List<ProviderProfile>> GetByCityAsync(string city)
        {
            return await _context.ProviderProfiles
                .Include(p => p.User)
                .Include(p => p.ServiceListings)
                .Where(p => p.City.ToLower() == city.ToLower())
                .ToListAsync();
        }

        public async Task<ProviderProfile?> GetByIdAsync(int id)
        {
            return await _context.ProviderProfiles
                .Include(p => p.User)
                .Include(p => p.ServiceListings)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<ProviderProfile?> GetByUserIdAsync(int userId)
        {
            return await _context.ProviderProfiles
                .Include(p => p.User)
                .Include(p => p.ServiceListings)
                .FirstOrDefaultAsync(p => p.UserId == userId);
        }

        public async Task AddAsync(ProviderProfile profile)
        {
            await _context.ProviderProfiles.AddAsync(profile);
        }

        public async Task UpdateAsync(ProviderProfile profile)
        {
            _context.ProviderProfiles.Update(profile);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}