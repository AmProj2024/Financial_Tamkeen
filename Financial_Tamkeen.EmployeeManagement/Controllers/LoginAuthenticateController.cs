using Financial_Tamkeen.EmployeeManagement.Models;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Financial_Tamkeen.EmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginAuthenticateController : ControllerBase
    {

        [HttpPost("Login")]
        public IActionResult Login(Login model)
        {
            // Authenticate user based on the provided username and password
            // You can use any authentication mechanism you prefer, such as checking against a database

            // If authentication is successful, generate a JWT token
            var token = GenerateJwtToken(model.UserName);

            return Ok(new { token });
        }

        private string GenerateJwtToken(string username)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username)
                };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-secret-key"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(1);

            var token = new JwtSecurityToken(
                "Jwt.issuer",
                "Jwt.Key",
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }

}
