using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hunar.Domain.Entities;

namespace Hunar.Domain.Interfaces
{
    public interface IProviderRepository
    {
        // Contract for all database operations related to ProviderProfiles

        Task<List<ProviderProfile>> GetAllAsync();
        Task<List<ProviderProfile>> GetByCategoryAsync(string category);
        Task<List<ProviderProfile>> GetByCityAsync(string category);
        Task<ProviderProfile?> GetByIdAsync(int id);
        Task<ProviderProfile?> GetByUserIdAsync(int userId);
        Task AddAsync(ProviderProfile profile);
        Task UpdateAsync(ProviderProfile profile);

        Task SaveChangesAsync();
       


    }
}
