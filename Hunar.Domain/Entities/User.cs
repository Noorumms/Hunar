using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hunar.Domain.Enums;

namespace Hunar.Domain.Entities
{
    // This represents anyone who has an account on Hunar
    // One User can be a Customer, Provider, or Admin
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty; // never store plain passwords
        public string Phone { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true; // Admin can deactivate accounts

        // Navigation property — if this user is a provider, their profile lives here
        public ProviderProfile? ProviderProfile { get; set; }

        // A customer can make many service requests
        public ICollection<ServiceRequest> ServiceRequests { get; set; } = new List<ServiceRequest>();
    }
}
