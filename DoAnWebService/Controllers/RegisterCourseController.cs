using DoAnWebService.Data;
using DoAnWebService.DTO.ResgisterCourse;
using DoAnWebService.DTO.Sinhvien;
using DoAnWebService.Models;
using DoAnWebService.Utlis;
using Microsoft.AspNetCore.Mvc;

namespace DoAnWebService.Controllers
{
    [Route("api/v1/private/[controller]")]
    [ApiController]
    public class RegisterCourseController : ControllerBase
    {
        private readonly QLSVContext _context;

        public RegisterCourseController(QLSVContext context)
        {
            _context = context;
        }

        [HttpPost("resgister-subject")]
        public async Task<IActionResult> ResigsterSubject(int maltc, string masv)
        {
            var student = await _context.Sinhviens.FindAsync(masv);
            if (student == null)
            {
                return NotFound(new ApiResponse<Sinhvien>
                {
                    Message = $"Không tìm thấy sinh viên {masv}.",
                    Data = null
                });
            }
            var course = await _context.Loptinchis.FindAsync(maltc);
            if (course == null)
            {
                return NotFound(new ApiResponse<Loptinchi>
                {
                    Message = $"Không tìm thấy lớp tín chỉ {maltc}.",
                    Data = null
                });
            }
            var subject = new Dangky
            {
                Masv = masv,
                Maltc = maltc
            };
            _context.Dangkies.Add(subject);
            await _context.SaveChangesAsync();
            return Ok(new ApiResponse<Dangky>
            {
                Message = $"Đăng ký lớp tín chỉ {maltc} cho sinh viên {masv} thành công.",
                Data = subject
            });
        }
    }
}
