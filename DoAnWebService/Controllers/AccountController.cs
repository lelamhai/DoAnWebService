using DoAnWebService.Data;
using DoAnWebService.DTO.Account;
using DoAnWebService.Models;
using DoAnWebService.Utlis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DoAnWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly QLSVContext _context;

        public AccountController(QLSVContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO model)
        {
            if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest(new ApiResponse<RegisterDTO>
                {
                    Message = "Username và Password không được để trống.",
                    Data = null
                });
            }

            if (_context.Users.Any(a => a.Username == model.Username))
            {
                return BadRequest(new ApiResponse<RegisterDTO>
                {
                    Message = "Username đã tồn tại.",
                    Data = null
                });
            }

            var newAccount = new User
            {
                Id = Guid.NewGuid(),
                Username = model.Username,
                Password = model.Password,
                Role = model.Role
            };
            var passwordHash = new PasswordHasher<User>().HashPassword(newAccount, model.Password);
            newAccount.Password = passwordHash;

            _context.Users.Add(newAccount);
            await _context.SaveChangesAsync();
            return Ok(new ApiResponse<RegisterDTO>
            {
                Message = $"Tạo mới tài khoản {model.Username} thành công.",
                Data = model
            });
        }
    }
}
