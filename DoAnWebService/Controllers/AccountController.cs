using DoAnWebService.Data;
using DoAnWebService.DTO.Account;
using DoAnWebService.Models;
using DoAnWebService.Utlis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DoAnWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly QLSVContext _context;
        private readonly IConfiguration _configuration;

        public AccountController(QLSVContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO model)
        {
            if(string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest(new ApiResponse<RegisterDTO>
                {
                    Message = "Username và Password không được để trống.",
                    Data = null
                });
            }

            if (_context.Accounts.Any(a => a.Username == model.Username))
            {
                return BadRequest(new ApiResponse<RegisterDTO>
                {
                    Message = "Username đã tồn tại.",
                    Data = null
                });
            }

            var newAccount = new Account
            {
                Username = model.Username,
                Role = model.Role,
                Active = true
            };
            var passwordHash = new PasswordHasher<Account>().HashPassword(newAccount, model.Password);
            newAccount.Password = passwordHash;

            _context.Accounts.Add(newAccount);
            await _context.SaveChangesAsync();
            return Ok(new ApiResponse<RegisterDTO>
            {
                Message = $"Tạo mới tài khoản {model.Username} thành công.",
                Data = model
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest(new ApiResponse<LoginDTO>
                {
                    Message = "Username và Password không được để trống.",
                    Data = null
                });
            }

            var account = _context.Accounts.FirstOrDefault(a => a.Username == model.Username);
            if (account == null || !account.Active)
            {
                return Unauthorized(new ApiResponse<LoginDTO>
                {
                    Message = "Sai username hoặc tài khoản không hoạt động.",
                    Data = null
                });
            }
            var passwordVerificationResult = new PasswordHasher<Account>().VerifyHashedPassword(account, account.Password, model.Password);
            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                return Unauthorized(new ApiResponse<LoginDTO>
                {
                    Message = "Mật khẩu không đúng.",
                    Data = null
                });
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, account.Username),
                new Claim(ClaimTypes.Role, account.Role)
            };

            var jwtKey = _configuration["Jwt:Key"];

            if (string.IsNullOrEmpty(jwtKey))
            {
                return StatusCode(500, new
                {
                    Message = "JWT Key chưa được cấu hình trong appsettings.json"
                });
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
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new ApiResponse<string>
            {
                Message = "Đăng nhập thành công.",
                Data = tokenString
            });
        }


        //private readonly UserManager<IdentityUser> _userManager;
        //private readonly RoleManager<IdentityRole> _roleManager;
        //private IConfiguration _configuration;

        //public AccountController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        //{
        //    _userManager = userManager;
        //    _roleManager = roleManager;
        //    _configuration = configuration;
        //}

        //[HttpPost("register")]
        //public async Task<IActionResult> Register(Register model)
        //{
        //    var user = new IdentityUser { UserName = model.Username };
        //    var result = await _userManager.CreateAsync(user, model.Password);

        //    if (result.Succeeded)
        //    {
        //        return Ok(new ApiResponse<Register>
        //        {
        //            Message = "Đăng ký thành công.",
        //            Data = null
        //        });
        //    }
        //    else
        //    {
        //        return BadRequest(new ApiResponse<Register>
        //        {
        //            Message = "Đăng ký thất bại: " + string.Join(", ", result.Errors.Select(e => e.Description)),
        //            Data = null
        //        });
        //    }
        //}

        //[HttpPost("login")]
        //public async Task<IActionResult> Login(Login login)
        //{
        //    var user = await _userManager.FindByNameAsync(login.Username);
        //    if (user != null && await _userManager.CheckPasswordAsync(user, login.Password))
        //    {
        //        var userRoles = await _userManager.GetRolesAsync(user);
        //        var authClaims = new List<Claim>
        //        {
        //            new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
        //            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //        };
        //        authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

        //        var token = new JwtSecurityToken(
        //            issuer: _configuration["Jwt:Issuer"],
        //            expires: DateTime.Now.AddMinutes(double.Parse(_configuration["Jwt:ExpireMinutes"]!)),
        //            claims: authClaims,
        //            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt: Key"]!)),
        //            SecurityAlgorithms.HmacSha256
        //            )
        //        );

        //        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        //        return Ok(new ApiResponse<string>
        //        {
        //            Message = "Đăng nhập thành công.",
        //            Data = tokenString
        //        });

        //    }
        //    else
        //    {
        //        return Unauthorized(new ApiResponse<Login>
        //        {
        //            Message = "Sai username hoặc password.",
        //            Data = null
        //        });
        //    }
        //}

        //[HttpPost("add-role")]
        //public async Task<IActionResult> AddRole(string role)
        //{
        //    if (!await _roleManager.RoleExistsAsync(role))
        //    {
        //        var result = await _roleManager.CreateAsync(new IdentityRole(role));
        //        if (result.Succeeded)
        //        {
        //            return Ok(new ApiResponse<string>
        //            {
        //                Message = "Thêm role thành công.",
        //                Data = null
        //            });
        //        }
        //        else
        //        {
        //            return BadRequest(new ApiResponse<string>
        //            {
        //                Message = "Thêm role thất bại: " + string.Join(", ", result.Errors.Select(e => e.Description)),
        //                Data = null
        //            });
        //        }
        //    }
        //    else
        //    {
        //        return BadRequest(new ApiResponse<string>
        //        {
        //            Message = "Role đã tồn tại.",
        //            Data = null
        //        });
        //    }
        //}

        //[HttpPost("assign-role")]
        //public async Task<IActionResult> AssignRole(UserRole role)
        //{
        //    var user = await _userManager.FindByNameAsync(role.Username);
        //    if (user == null)
        //    {
        //        return NotFound(new ApiResponse<string>
        //        {
        //            Message = "Người dùng không tồn tại.",
        //            Data = null
        //        });
        //    }

        //    var result = await _userManager.AddToRoleAsync(user, role.Role);
        //    if (result.Succeeded)
        //    {
        //        return Ok(new ApiResponse<string>
        //        {
        //            Message = "Gán role thành công.",
        //            Data = null
        //        });
        //    }
        //    else
        //    {
        //        return BadRequest(new ApiResponse<string>
        //        {
        //            Message = "Gán role thất bại: " + string.Join(", ", result.Errors.Select(e => e.Description)),
        //            Data = null
        //        });
        //    }
        //}
    }
}
