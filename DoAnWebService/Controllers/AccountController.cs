using DoAnWebService.Data;
using DoAnWebService.DTO.Account;
using DoAnWebService.Models;
using DoAnWebService.Utlis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAnWebService.Controllers
{
    [Route("api/v1/private/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly QLSVContext _context;

        public AccountController(QLSVContext context)
        {
            _context = context;
        }

        [HttpPost("create-account")]
        public async Task<IActionResult> CreateAccount(CreateAccountDTO model)
        {
            if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest(new APIResponse<CreateAccountDTO>
                {
                    Message = "Username và Password không được để trống.",
                    Data = null
                });
            }

            if (_context.Users.Any(a => a.Username == model.Username))
            {
                return BadRequest(new APIResponse<CreateAccountDTO>
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
            return Ok(new APIResponse<CreateAccountDTO>
            {
                Message = $"Tạo mới tài khoản {model.Username} thành công.",
                Data = model
            });
        }

        [HttpGet("info-account")]
        public async Task<IActionResult> InfoAccount(string username)
        {
            username = username.Trim();

            // Kiểm tra Nhân viên
            var nhanVien = await _context.Nhanviens
                .FirstOrDefaultAsync(x => x.Manv == username);

            if (nhanVien != null)
            {
                return Ok(new APIResponse<Nhanvien>
                {
                    Message = $"Thông tin tài khoản thành công.",
                    Data = nhanVien
                });
            }

            // Kiểm tra Giảng viên
            var giangVien = await _context.Giangviens
                .FirstOrDefaultAsync(x => x.Magv == username);

            if (giangVien != null)
            {
                return Ok(new APIResponse<Giangvien>
                {
                    Message = $"Thông tin tài khoản thành công.",
                    Data = giangVien
                });
            }

            // Kiểm tra Sinh viên
            var sinhVien = await _context.Sinhviens
                .FirstOrDefaultAsync(x => x.Masv == username);

            if (sinhVien != null)
            {
                return Ok(new APIResponse<Sinhvien>
                {
                    Message = $"Thông tin tài khoản thành công.",
                    Data = sinhVien
                });
            }

            return NotFound(new APIResponse<Sinhvien>
            {
                Message = $"Không tìm thấy thông tin tài khoản.",
                Data = null
            });
        }
    }
}
