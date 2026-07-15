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

            UserInfoModel userInfo = new UserInfoModel();
            if (nhanVien != null)
            {
                userInfo.Username = nhanVien.Manv;
                userInfo.Ho = nhanVien.Ho;
                userInfo.Ten = nhanVien.Ten;
                userInfo.Trangthai = nhanVien.Trangthai;
                return Ok(new APIResponse<UserInfoModel>
                {
                    Message = $"Thông tin tài khoản thành công.",
                    Data = userInfo
                });
            }

            // Kiểm tra Giảng viên
            var giangVien = await _context.Giangviens
                .FirstOrDefaultAsync(x => x.Magv == username);

            if (giangVien != null)
            {
                userInfo.Username = giangVien.Magv;
                userInfo.Ho = giangVien.Ho;
                userInfo.Ten = giangVien.Ten;
                userInfo.Trangthai = giangVien.Trangthai;
                return Ok(new APIResponse<UserInfoModel>
                {
                    Message = $"Thông tin tài khoản thành công.",
                    Data = userInfo
                });
            }

            // Kiểm tra Sinh viên
            var sinhVien = await _context.Sinhviens
                .FirstOrDefaultAsync(x => x.Masv == username);

            if (sinhVien != null)
            {
                userInfo.Username = sinhVien.Masv;
                userInfo.Ho = sinhVien.Ho;
                userInfo.Ten = sinhVien.Ten;
                userInfo.Trangthai = sinhVien.Trangthai;
                return Ok(new APIResponse<UserInfoModel>
                {
                    Message = $"Thông tin tài khoản thành công.",
                    Data = userInfo
                });
            }

            return NotFound(new APIResponse<string>
            {
                Message = $"Không tìm thấy thông tin tài khoản.",
                Data = null
            });
        }
    }
}
