using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hunar.Application.DTOs
{
    // What provider sends when creating their profile
    public class CreateProviderProfileDTO
    {
        public string Bio { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty; // e.g. Electrician
        public string City { get; set; } = string.Empty;
        public string Area { get; set; } = string.Empty;
    }

    // What provider sends when adding a service listing
    public class CreateServiceListingDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }

    // What we send BACK when someone views a provider
    // Clean shape — only what frontend needs
    public class ProviderProfileResponseDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Area { get; set; } = string.Empty;
        public double AverageRating { get; set; }
        public int TotalReviews { get; set; }
        public string Availability { get; set; } = string.Empty;
        public List<ServiceListingResponseDTO> Services { get; set; } = new();
    }

    // Individual service listing shape
    public class ServiceListingResponseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
    }
}