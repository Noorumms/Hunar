using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hunar.Application.DTOs;
using Hunar.Domain.Entities;
using Hunar.Domain.Enums;
using Hunar.Domain.Interfaces;

namespace Hunar.Application.UseCases.Provider
{
    public class CreateProviderProfileUseCase
    {
        private readonly IProviderRepository _providerRepository;
        private readonly IUserRepository _userRepository;

        public CreateProviderProfileUseCase(
            IProviderRepository providerRepository,
            IUserRepository userRepository)
        {
            _providerRepository = providerRepository;
            _userRepository = userRepository;
        }

        // userId comes from the JWT token — not from the request body
        // This prevents users from creating profiles for other users
        public async Task<string> ExecuteAsync(int userId, CreateProviderProfileDTO dto)
        {
            // ── RULE 1: User must exist ───────────────────────────
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            // ── RULE 2: Must be a Provider role ───────────────────
            if (user.Role != UserRole.Provider)
                throw new InvalidOperationException(
                    "Only Provider accounts can create a service profile.");

            // ── RULE 3: Can't create duplicate profile ────────────
            var existingProfile = await _providerRepository.GetByUserIdAsync(userId);
            if (existingProfile != null)
                throw new InvalidOperationException(
                    "You already have a provider profile.");

            // ── CREATE PROFILE ────────────────────────────────────
            var profile = new ProviderProfile
            {
                UserId = userId,
                Bio = dto.Bio,
                Category = dto.Category,
                City = dto.City,
                Area = dto.Area,
                Availability = AvailabilityStatus.Available,
                AverageRating = 0.0,
                TotalReviews = 0
            };

            await _providerRepository.AddAsync(profile);
            await _providerRepository.SaveChangesAsync();

            return "Provider profile created successfully.";
        }
    }
}
