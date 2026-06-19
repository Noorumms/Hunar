using Hunar.Domain.Entities;
using Hunar.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hunar.Domain.Enums;

namespace Hunar.Domain.Entities
{
    // The core transaction of the platform
    // A customer asking a provider to do a job
    public class ServiceRequest
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty; // what the customer needs
        public string PreferredDate { get; set; } = string.Empty;
        public RequestStatus Status { get; set; } = RequestStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } // nullable — only set when status changes

        // //foreign key to user (cutomer) who reuested the service 
        public int CustomerId { get; set; }
        public User Customer { get; set; } = null!;

        //foreign key to user(provider) to whom the request was sent to 
        public int ProviderProfileId { get; set; }
        public ProviderProfile ProviderProfile { get; set; } = null!;

        // Review left after completion — nullable because job may not be done yet
        public Review? Review { get; set; }
    }
}

