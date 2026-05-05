using DoAnWebService.Data;
using DoAnWebService.DTO.Account;
using DoAnWebService.DTO.User;
using DoAnWebService.Models;
using DoAnWebService.Utils;
using DoAnWebService.Utlis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DoAnWebService.Controllers
{
    [Route("api/v1/public/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly QLSVContext _context;
        private readonly IConfiguration _configuration;

        public UserController(QLSVContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginResponseDTO model)
        {
            if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest(new ApiResponse<LoginResponseDTO>
                {
                    Message = "Username và Password không được để trống.",
                    Data = null
                });
            }

            var account = _context.Users.FirstOrDefault(a => a.Username == model.Username);
            if (account == null)
            {
                return Unauthorized(new ApiResponse<LoginResponseDTO>
                {
                    Message = "Sai username hoặc tài khoản không hoạt động.",
                    Data = null
                });
            }
            var passwordVerificationResult = new PasswordHasher<User>().VerifyHashedPassword(account, account.Password, model.Password);
            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                return Unauthorized(new ApiResponse<LoginResponseDTO>
                {
                    Message = "Mật khẩu không đúng.",
                    Data = null
                });
            }


            var expiry = Convert.ToInt32(_configuration["Jwt:ExpireDays"]);
            var tokenString = CreateToken(account);
            var refreshToken = GenerateRefreshToken();


            account.Refreshtoken = refreshToken;
            account.Expiry = DateTime.Now.AddDays(expiry);
            await _context.SaveChangesAsync();
            return Ok(new ApiResponse<string>
            {
                Message = "Đăng nhập thành công.",
                Data = tokenString
            });
        }

        private string CreateToken(User account)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, account.Username),
                new Claim(ClaimTypes.Role, account.Role)
            };

            var jwtKey = _configuration["Jwt:Key"];

            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new Exception("JWT Key chưa được cấu hình trong appsettings.json");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

            var signIn = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(
                    Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])
                ),
                signingCredentials: signIn
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
