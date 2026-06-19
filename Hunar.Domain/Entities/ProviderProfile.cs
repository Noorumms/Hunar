using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Hunar.Domain.Enums;

namespace Hunar.Domain.Entities
{
    // Extra details that only exist if the user is a Provider
    // Separated from User so Customers don't carry unnecessary data
    public class ProviderProfile
    {
        public int Id { get; set; }
        public string Bio { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty; // e.g. "Electrician", "Plumber"
        public string City { get; set; } = string.Empty;
        public string Area { get; set; } = string.Empty;     // hyperlocal — neighbourhood level
        public double AverageRating { get; set; } = 0.0;
        public int TotalReviews { get; set; } = 0;
        public AvailabilityStatus Availability { get; set; } = AvailabilityStatus.Available;

        // Foreign key back to User
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        // A provider can offer many services
        public ICollection<ServiceListing> ServiceListings { get; set; } = new List<ServiceListing>();

        // A provider receives many service requests
        public ICollection<ServiceRequest> ServiceRequests { get; set; } = new List<ServiceRequest>();
    }
}
