using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hunar.Domain.Interfaces
{
    // Application will call this interface
    // API layer will implement it
    // Application never touches JWT packages directly
    public interface IJwtService
    {
        string GenerateToken(int userId, string email, string role);
    }
}
