using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hunar.Domain.Entities
{
    public class Review
    {

        // Feedback a customer leaves after a job is completed
        // Only one review per service request

        public int Id { get; set; }
        public int Rating { get; set; }   // 1 to 5
        public string Comment { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Which request this review belongs to
        public int ServiceRequestId { get; set; }
        public ServiceRequest ServiceRequest { get; set; } = null;
    }
}
