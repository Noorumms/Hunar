using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Why enums? Instead of storing strings like "Pending" or "Confirmed" 
//    in the database (which can be misspelled or inconsistent), 
//    we store numbers (0,1,2,3) and map them to meaningful names in code. 
//    Safer, faster, cleaner.
namespace Hunar.Domain.Enums
{
    // What role does this user have on the platform?
    public enum UserRole
    {
        Customer,
        Provider,
        Admin
    }

    // What is the current state of a service request?
    // This is the core workflow of the entire app
    public enum RequestStatus
    {
        Pending,    // Customer just submitted it
        Confirmed,  // Provider accepted it
        Completed,  // Job is done
        Cancelled   // Either side cancelled
    }

    // How available is the provider right now?
    public enum AvailabilityStatus
    {
        Available,
        Busy,
        Inactive
    }
}

