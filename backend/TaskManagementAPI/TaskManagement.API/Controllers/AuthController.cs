using Microsoft.AspNetCore.Mvc;
using TaskManagement.API.Services;
using TaskManagement.Application.DTOs.Auth;

namespace TaskManagement.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Permite autenticarse y generar un token JWT.
        /// </summary>
        /// <remarks>
        /// Usuario demo:
        /// - Username: admin
        /// - Password: admin123
        /// </remarks>
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDto request)
        {
            var result = _authService.Login(request);
            return Ok(result);
        }
    }
}
