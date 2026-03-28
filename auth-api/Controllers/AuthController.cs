using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using auth_api.DTOs;
using auth_api.Models;
using auth_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace auth_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;

        public AuthController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok(new { status = "Auth API is running" });
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
            {
                return BadRequest(new { message = "Email and password are required." });
            }

            var existingUser = UserStore.Users.FirstOrDefault(u => u.Email.ToLower() == dto.Email.ToLower());
            if (existingUser != null)
            {
                return BadRequest(new { message = "Email already exists." });
            }

            var user = new User
            {
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = string.IsNullOrWhiteSpace(dto.Role) ? "player" : dto.Role.ToLower()
            };

            UserStore.Users.Add(user);

            return Ok(new
            {
                message = "User registered successfully.",
                user = new
                {
                    user.Id,
                    user.Email,
                    user.Role
                }
            });
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            var user = UserStore.Users.FirstOrDefault(u => u.Email.ToLower() == dto.Email.ToLower());

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }

            var token = _tokenService.CreateToken(user);

            return Ok(new
            {
                token,
                user = new
                {
                    id = user.Id,
                    email = user.Email,
                    role = user.Role
                }
            });
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            var id = User.FindFirstValue(JwtRegisteredClaimNames.Sub) 
                      ?? User.FindFirstValue(ClaimTypes.NameIdentifier);

            var email = User.FindFirstValue(JwtRegisteredClaimNames.Email) 
                        ?? User.FindFirstValue(ClaimTypes.Email);

            var role = User.FindFirstValue(ClaimTypes.Role);

            return Ok(new
            {
                id,
                email,
                role
            });
        }

        [Authorize]
        [HttpGet("user/{id}")]
        public IActionResult GetUser(Guid id)
        {
            var user = UserStore.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            return Ok(new
            {
                user.Id,
                user.Email,
                user.Role
            });
        }
    }
}