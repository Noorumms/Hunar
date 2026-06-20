using Hunar.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hunar.Domain.Interfaces;
using Hunar.Domain.Entities;
using Hunar.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hunar.Infrastructure.Repositories
{
    // This is the IMPLEMENTATION of IUserRepository
    // Domain defined the contract, we fulfill it here
    // EF Core does the actual SQL work
    public class UserRepository : IUserRepository
    {
        private readonly HunarDbContext _context;

        // DbContext injected via constructor — .NET handles this
        public UserRepository(HunarDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            // FirstOrDefaultAsync returns null if not found
            // instead of throwing an exception
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<List<User>> GetAllAsync()
        {
            // ToListAsync loads everything into memory
            // Fine for admin panel, not for huge datasets
            return await _context.Users.ToListAsync();
        }

        public async Task AddAsync(User user)
        {
            // Marks the entity as "Added" in EF's change tracker
            // Nothing hits the DB yet — SaveChanges does that
            await _context.Users.AddAsync(user);
        }

        public async Task UpdateAsync(User user)
        {
            // Marks entity as "Modified" in change tracker
            _context.Users.Update(user);
        }

        public async Task SaveChangesAsync()
        {
            // THIS is when EF actually runs the SQL
            // All tracked changes get committed in one transaction
            await _context.SaveChangesAsync();
        }
    }
}