using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hunar.Application.DTOs;
using Hunar.Domain.Entities;
using Hunar.Domain.Interfaces;

namespace Hunar.Application.UseCases.Provider
{
    public class AddServiceListingUseCase
    {
        private readonly IProviderRepository _providerRepository;

        public AddServiceListingUseCase(IProviderRepository providerRepository)
        {
            _providerRepository = providerRepository;
        }

        public async Task<string> ExecuteAsync(int userId, CreateServiceListingDTO dto)
        {
            // ── RULE 1: Provider profile must exist ───────────────
            var profile = await _providerRepository.GetByUserIdAsync(userId);
            if (profile == null)
                throw new KeyNotFoundException(
                    "Provider profile not found. Please create your profile first.");

            // ── RULE 2: Price must be positive ────────────────────
            if (dto.Price <= 0)
                throw new ArgumentException("Price must be greater than zero.");

            // ── ADD LISTING ───────────────────────────────────────
            var listing = new ServiceListing
            {
                Title = dto.Title,
                Description = dto.Description,
                Price = dto.Price,
                IsActive = true,
                ProviderProfileId = profile.Id
            };

            // Add to the profile's collection
            // EF tracks this and saves it when we call SaveChanges
            profile.ServiceListings.Add(listing);
            await _providerRepository.SaveChangesAsync();

            return "Service listing added successfully.";
        }
    }
}