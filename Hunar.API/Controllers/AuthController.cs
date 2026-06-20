using Hunar.Application.DTOs;
using Hunar.Application.UseCases.Auth;
using Microsoft.AspNetCore.Mvc;


namespace Hunar.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly RegisterUseCase _registerUseCase;
        private readonly LoginUseCase _loginUseCase;

        // Both use cases injected automatically by .NET
        public AuthController(RegisterUseCase registerUseCase, LoginUseCase loginUseCase)
        {
            _registerUseCase = registerUseCase;
            _loginUseCase = loginUseCase;
        }

        // POST api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            // [FromBody] means read the data from the request body JSON
            var result = await _registerUseCase.ExecuteAsync(dto);
            return Ok(new { message = result });
        }

        // POST api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            var result = await _loginUseCase.ExecuteAsync(dto);
            return Ok(result);
        }
    }
}
