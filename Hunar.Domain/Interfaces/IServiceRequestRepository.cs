using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hunar.Domain.Entities;
using Hunar.Domain.Enums;

namespace Hunar.Domain.Interfaces
{
    public interface IServiceRequestRepository
    {
        Task<List<ServiceRequest>> GetAllAsync();
        Task<List<ServiceRequest>> GetByCustomerIdAsync(int customerId);
        Task<List<ServiceRequest>> GetByProviderIdAsync(int providerProfileId);
        Task<List<ServiceRequest>> GetByStatusAsync(RequestStatus status);
        Task<ServiceRequest?> GetByIdAsync(int id);
        Task AddAsync(ServiceRequest request);
        Task UpdateAsync(ServiceRequest request);
        Task SaveChangesAsync();
    }
}
