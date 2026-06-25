using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hunar.Application.DTOs;
using Hunar.Domain.Interfaces;

namespace Hunar.Application.UseCases.Provider
{
    public class GetProvidersUseCase
    {
        private readonly IProviderRepository _providerRepository;

        public GetProvidersUseCase(IProviderRepository providerRepository)
        {
            _providerRepository = providerRepository;
        }

        // Optional filters — if null, return everything
        public async Task<List<ProviderProfileResponseDTO>> ExecuteAsync(
            string? category, string? city)
        {
            // LINQ to filter based on what was provided
            var providers = category != null
                ? await _providerRepository.GetByCategoryAsync(category)
                : city != null
                    ? await _providerRepository.GetByCityAsync(city)
                    : await _providerRepository.GetAllAsync();

            // Map domain entities to response DTOs
            // We never return raw entities to the client
            return providers.Select(p => new ProviderProfileResponseDTO
            {
                Id = p.Id,
                FullName = p.User.FullName,
                Bio = p.Bio,
                Category = p.Category,
                City = p.City,
                Area = p.Area,
                AverageRating = p.AverageRating,
                TotalReviews = p.TotalReviews,
                Availability = p.Availability.ToString(),
                Services = p.ServiceListings
                    .Where(s => s.IsActive)
                    .Select(s => new ServiceListingResponseDTO
                    {
                        Id = s.Id,
                        Title = s.Title,
                        Description = s.Description,
                        Price = s.Price,
                        IsActive = s.IsActive
                    }).ToList()
            }).ToList();
        }
    }
}