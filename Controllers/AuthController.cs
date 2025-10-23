using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace InventorySystem_webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IConfiguration config) : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var validUser = config["LOGIN_USER"];
            var validPassword = config["LOGIN_PASSWORD"];

            if (request.Username != validUser || request.Password != validPassword)
                return Unauthorized(new { message = "Invalid credentials" });

            var token = GenerateJwtToken(request.Username);
            return Ok(new { token });
        }

        private string GenerateJwtToken(string username)
        {
            var jwtSettings = config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new("sub", username),
                new("role", "Admin"),
                new("jti", Guid.NewGuid().ToString())
            };

            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpireMinutes"])),
                SigningCredentials = creds
            };

            var handler = new JsonWebTokenHandler();
            return handler.CreateToken(descriptor);
        }
    }

    public record LoginRequest(string Username, string Password);
}