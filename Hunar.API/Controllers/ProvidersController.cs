using Hunar.Application.UseCases.Provider;
using Hunar.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hunar.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProvidersController : ControllerBase
    {
        private readonly GetProvidersUseCase _getProvidersUseCase;
        private readonly CreateProviderProfileUseCase _createProviderProfileUseCase;
        private readonly AddServiceListingUseCase _addServiceListingUseCase;

        public ProvidersController(
            GetProvidersUseCase getProvidersUseCase,
            CreateProviderProfileUseCase createProviderProfileUseCase,
            AddServiceListingUseCase addServiceListingUseCase)
        {
            _getProvidersUseCase = getProvidersUseCase;
            _createProviderProfileUseCase = createProviderProfileUseCase;
            _addServiceListingUseCase = addServiceListingUseCase;
        }

        // GET api/providers
        // GET api/providers?category=Electrician
        // GET api/providers?city=Lahore
        // Public — anyone can browse providers without logging in
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? category,
            [FromQuery] string? city)
        {
            var result = await _getProvidersUseCase.ExecuteAsync(category, city);
            return Ok(result);
        }

        // POST api/providers/profile
        // Only Provider role can access this
        // [Authorize] means JWT token required
        // [Authorize(Roles = "Provider")] means token AND correct role
        [HttpPost("profile")]
        [Authorize(Roles = "Provider")]
        public async Task<IActionResult> CreateProfile([FromBody] CreateProviderProfileDTO dto)
        {
            // Extract userId from the JWT token claims
            // The token has userId embedded — we don't trust the client to send it
            var userId = GetUserIdFromToken();
            var result = await _createProviderProfileUseCase.ExecuteAsync(userId, dto);
            return Ok(new { message = result });
        }

        // POST api/providers/services
        // Add a new service listing
        [HttpPost("services")]
        [Authorize(Roles = "Provider")]
        public async Task<IActionResult> AddService([FromBody] CreateServiceListingDTO dto)
        {
            var userId = GetUserIdFromToken();
            var result = await _addServiceListingUseCase.ExecuteAsync(userId, dto);
            return Ok(new { message = result });
        }

        // Helper method — reads userId from JWT claims
        // ClaimTypes.NameIdentifier is where we stored userId
        private int GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            if (userIdClaim == null)
                throw new UnauthorizedAccessException("Invalid token.");
            return int.Parse(userIdClaim);
        }
    }
}
