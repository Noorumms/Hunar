using Hunar.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hunar.Domain.Interfaces
{
    // Contract for review operations
    public interface IReviewRepository
    {

        // Contract for review operations
        public interface IReviewRepository
        {
            Task<List<Review>> GetByProviderIdAsync(int providerProfileId);
            Task<Review?> GetByServiceRequestIdAsync(int serviceRequestId);
            Task AddAsync(Review review);
            Task SaveChangesAsync();
        }
    }
}
