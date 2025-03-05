using Microsoft.AspNetCore.Mvc;
using Api_ECommerce.DTO;
using Microsoft.AspNetCore.Identity;
using Api_ECommerce.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<Appuser> userManager;
        private readonly IConfiguration config;

        public AccountController(UserManager<Appuser> userManager, IConfiguration config)
        {
            this.userManager = userManager;
            this.config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Registration([FromForm] RegisterUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Appuser user = new Appuser
            {
                UserName = userDto.UserName,
                Email = userDto.Email
            };

            IdentityResult result = await userManager.CreateAsync(user, userDto.Password);
            if (result.Succeeded)
            {
                return Ok("Account created successfully.");
            }

            return BadRequest(result.Errors.Select(e => e.Description));
        }

        // POST: /api/Account/Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await userManager.FindByNameAsync(userDto.UserName);
            if (user == null || !await userManager.CheckPasswordAsync(user, userDto.Password))
            {
                return Unauthorized("Invalid username or password.");
            }

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: config["JWT:ValidIssuer"],
                audience: config["JWT:ValidAudience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor),
                expiration = tokenDescriptor.ValidTo
            });
        }
    }
}
