using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hunar.Domain.Entities;
using Hunar.Domain.Interfaces;
using Hunar.Application.DTOs;

using System.Runtime.CompilerServices;
using System.Security.Claims;


namespace Hunar.Application.UseCases.Auth
{
    public class LoginUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        // Notice — no IConfiguration here anymore
        // Application doesn't care HOW the token is made
        // It just calls the interface
        public LoginUseCase(IUserRepository userRepository, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public async Task<AuthResponseDTO> ExecuteAsync(LoginDTO dto)
        {
            // ── STEP 1: Find the user ─────────────────────────────
            var user = await _userRepository.GetByEmailAsync(dto.Email.ToLower().Trim());

            if (user == null || !user.IsActive)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            // ── STEP 2: Verify password ───────────────────────────
            var passwordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            if (!passwordValid)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            // ── STEP 3: Generate token via interface ──────────────
            // We don't know or care how the token is generated
            // That's the API layer's job
            var token = _jwtService.GenerateToken(user.Id, user.Email, user.Role.ToString());

            return new AuthResponseDTO
            {
                Token = token,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role.ToString(),
                UserId = user.Id
            };
        }
    }
}