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
        public async Task<IActionResult> Login(LoginRequestDTO model)
        {
            if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest(new APIResponse<LoginResponseDTO>
                {
                    Message = "Username và Password không được để trống.",
                    Data = null
                });
            }

            var account = await _context.Users
                .FirstOrDefaultAsync(a => a.Username == model.Username);

            if (account == null)
            {
                return Unauthorized(new APIResponse<LoginResponseDTO>
                {
                    Message = "Sai username hoặc tài khoản không hoạt động.",
                    Data = null
                });
            }

            var passwordVerificationResult =
                new PasswordHasher<User>().VerifyHashedPassword(
                    account,
                    account.Password,
                    model.Password
                );

            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                return Unauthorized(new APIResponse<LoginResponseDTO>
                {
                    Message = "Mật khẩu không đúng.",
                    Data = null
                });
            }

            var accessToken = CreateToken(account);
            var refreshToken = GenerateRefreshToken();

            int refreshTokenExpireDays = Convert.ToInt32(_configuration["Jwt:RefreshTokenExpireDays"]);

            account.Refreshtoken = refreshToken;
            account.Expiry = DateTime.UtcNow.AddDays(refreshTokenExpireDays);

            await _context.SaveChangesAsync();

            return Ok(new APIResponse<LoginResponseDTO>
            {
                Message = "Đăng nhập thành công.",
                Data = new LoginResponseDTO
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    Role = account.Role,
                }
            });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(LoginResponseDTO model)
        {
            if (string.IsNullOrEmpty(model.AccessToken) || string.IsNullOrEmpty(model.RefreshToken))
            {
                return BadRequest(new APIResponse<LoginResponseDTO>
                {
                    Message = "Access token và refresh token không được để trống.",
                    Data = null
                });
            }

            ClaimsPrincipal principal;

            try
            {
                principal = GetPrincipalFromExpiredToken(model.AccessToken);
            }
            catch
            {
                return Unauthorized(new APIResponse<LoginResponseDTO>
                {
                    Message = "Access token không hợp lệ.",
                    Data = null
                });
            }

            var username = principal.Identity?.Name;

            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized(new APIResponse<LoginResponseDTO>
                {
                    Message = "Không tìm thấy username trong token.",
                    Data = null
                });
            }

            var account = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);

            if (account == null)
            {
                return Unauthorized(new APIResponse<LoginResponseDTO>
                {
                    Message = "Tài khoản không tồn tại.",
                    Data = null
                });
            }

            if (account.Refreshtoken != model.RefreshToken)
            {
                return Unauthorized(new APIResponse<LoginResponseDTO>
                {
                    Message = "Refresh token không hợp lệ.",
                    Data = null
                });
            }

            if (account.Expiry <= DateTime.UtcNow)
            {
                return Unauthorized(new APIResponse<LoginResponseDTO>
                {
                    Message = "Refresh token đã hết hạn. Vui lòng đăng nhập lại.",
                    Data = null
                });
            }

            var newAccessToken = CreateToken(account);
            var newRefreshToken = GenerateRefreshToken();

            int refreshTokenExpireDays = Convert.ToInt32(_configuration["Jwt:RefreshTokenExpireDays"]);

            account.Refreshtoken = newRefreshToken;
            account.Expiry = DateTime.UtcNow.AddDays(refreshTokenExpireDays);

            await _context.SaveChangesAsync();

            return Ok(new APIResponse<LoginResponseDTO>
            {
                Message = "Refresh token thành công.",
                Data = new LoginResponseDTO
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                    ExpiredToken = DateTime.UtcNow.AddDays(refreshTokenExpireDays)
                }
            });
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout(LogoutDTO model)
        {
            if (string.IsNullOrWhiteSpace(model.username))
            {
                return BadRequest(new APIResponse<string>
                {
                    Message = "Username không được để trống.",
                    Data = null
                });
            }


            var account = await _context.Users
                .FirstOrDefaultAsync(
                    u => u.Username == model.username);


            if (account != null)
            {
                account.Refreshtoken = null;
                account.Expiry = null;

                await _context.SaveChangesAsync();
            }


            return Ok(new APIResponse<string>
            {
                Message = "Đăng xuất thành công.",
                Data = null
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

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)
            );

            var signIn = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            var expireMinutes = Convert.ToDouble(_configuration["Jwt:ExpireMinutes"]);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expireMinutes),
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

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var jwtKey = _configuration["Jwt:Key"];

            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new Exception("JWT Key chưa được cấu hình trong appsettings.json");
            }

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"],

                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtKey)
                ),

                // Cho phép đọc token dù access token đã hết hạn
                ValidateLifetime = false,

                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var principal = tokenHandler.ValidateToken(
                token,
                tokenValidationParameters,
                out SecurityToken securityToken
            );

            if (securityToken is not JwtSecurityToken jwtSecurityToken)
            {
                throw new SecurityTokenException("Token không hợp lệ.");
            }

            if (!jwtSecurityToken.Header.Alg.Equals(
                    SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Thuật toán token không hợp lệ.");
            }

            return principal;
        }
    }
}