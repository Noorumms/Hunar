using Hunar.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hunar.Domain.Entities;
using Hunar.Domain.Enums;
using Hunar.Application.DTOs;

namespace Hunar.Application.UseCases.Auth
{
    public class RegisterUseCase
    {
        // We depend on the INTERFACE not the concrete class
        // Application never knows about Entity Framework directly
        private readonly IUserRepository _userRepository;

        // Constructor injection — .NET gives us the repository automatically
        public RegisterUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<string> ExecuteAsync(RegisterDTO dto)
        {
            // ── RULE 1: Email must be unique ──────────────────────
            // Check if someone already registered with this email
            var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
            if (existingUser != null)
            {
                // Throw exception — our middleware will catch this
                // and return a proper 400 response
                throw new InvalidOperationException("An account with this email already exists.");
            }

            // ── RULE 2: Role must be valid ────────────────────────
            // Only Customer or Provider can self-register
            // Admin accounts are created manually — never via API
            if (!Enum.TryParse<UserRole>(dto.Role, true, out var role) ||
                role == UserRole.Admin)
            {
                throw new InvalidOperationException("Role must be either Customer or Provider.");
            }

            // ── RULE 3: Hash the password ─────────────────────────
            // NEVER store plain text passwords
            // BCrypt adds a random salt and hashes — even same password
            // gives different hash each time
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            // ── CREATE THE USER ───────────────────────────────────
            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email.ToLower().Trim(), // normalize email
                PasswordHash = passwordHash,
                Phone = dto.Phone,
                Role = role,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            // If registering as Provider, create an empty profile for them
            // They can fill it in later — but the profile needs to exist
            // We return a simple success message here
            // The JWT token is only given on LOGIN not registration
            return "Registration successful. Please login to continue.";
        }
    }
}