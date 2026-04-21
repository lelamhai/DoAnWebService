using DoAnWebService.Data;
using DoAnWebService.DTO.Sinhvien;
using DoAnWebService.DTO.User;
using DoAnWebService.Models;
using DoAnWebService.Utlis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAnWebService.Controllers
{
    [Route("api/v1/public/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly QLSVContext _context;

        public UserController(QLSVContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserDTO userDTO)
        {
            if (userDTO == null)
            {
                return BadRequest(new ApiResponse<User>
                {
                    Message = "Dữ liệu gửi lên không hợp lệ.",
                    Data = null
                });
            }

            if (string.IsNullOrWhiteSpace(userDTO.UserName) || string.IsNullOrWhiteSpace(userDTO.Password))
            {
                return BadRequest(new ApiResponse<User>
                {
                    Message = "Username và password không được để trống.",
                    Data = null
                });
            }

            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == userDTO.UserName && x.Password == userDTO.Password);
            if (user == null)
            {
                return Unauthorized(new ApiResponse<User>
                {
                    Message = "Sai username hoặc password.",
                    Data = null
                });
            }

            return Ok(new ApiResponse<User>
            {
                Message = "Đăng nhập thành công.",
                Data = user
            });
        }


    }
}
