using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hunar.Application.DTOs
{
    //WHat clint sends while registering 
     // we only ask for what we absolutely need 
    public class RegisterDTO
    {
        public string FullName { get; set; }
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        //client tells who they are customer or provider 

        public string Role { get; set; } = string.Empty;

    }

    //when client sends when logging in 
    public class LoginDTO
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;


    }

    //What we send back after successful login 
    //no password or sensitive data - just what needee


    public class AuthResponseDTO
    {
        public string Token { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public int UserId { get; set; }

    }

}
