using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hunar.Domain.Entities
{
    // A specific service a provider offers with a price
    // e.g. "Fan Installation - Rs.300" or "Maths Tuition - Rs.800/hr"
    public class ServiceListing
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsActive { get; set; } = true;

        // Foreign key to ProviderProfile
        public int ProviderProfileId { get; set; }
        public ProviderProfile ProviderProfile { get; set; } = null!;
    }
}
