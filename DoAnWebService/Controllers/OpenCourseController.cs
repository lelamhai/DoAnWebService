using DoAnWebService.Data;
using DoAnWebService.DTO.OpenCourse;
using DoAnWebService.Models;
using DoAnWebService.Utils;
using DoAnWebService.Utlis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAnWebService.Controllers
{
    [Route("api/v1/private/[controller]")]
    [ApiController]
    public class OpenCourseController : ControllerBase
    {
        private readonly QLSVContext _context;

        public OpenCourseController(QLSVContext context)
        {
            _context = context;
        }

        [HttpGet("get-open-courses")]
        public async Task<IActionResult> GetOpenCourses(int page = 1)
        {
            var coursesSection = await _context.Loptinchis.ToListAsync();
            var result = PaginationHelper.CreatePagedResult(coursesSection, page, 3);
            return Ok(new ApiResponse<PagedResult<Loptinchi>>
            {
                Message = "Lấy danh sách lớp tín chỉ thành công.",
                Data = result
            });
        }

        [HttpGet("detail-open-course/{maltc}")]
        public async Task<ActionResult<Loptinchi>> GetOpenCourseByMaLTC(int maltc)
        {
            var course = await _context.Loptinchis.FirstOrDefaultAsync(sv => sv.Maltc == maltc);

            if (course == null)
            {
                return NotFound(new ApiResponse<Loptinchi>
                {
                    Message = $"Không tìm mã lớp tín chỉ {maltc}.",
                    Data = null
                });
            }

            return Ok(new ApiResponse<Loptinchi>
            {
                Message = $"Tìm thấy mã lớp tín chỉ {maltc} thành công",
                Data = course
            });
        }

        [HttpDelete("delete-open-course/{maltc}")]
        public async Task<IActionResult> DeleteOpenCourse(int maltc)
        {
            var course = await _context.Loptinchis.FirstOrDefaultAsync(x => x.Maltc == maltc);

            if (course == null)
            {
                return NotFound(new ApiResponse<Loptinchi>
                {
                    Message = $"Không tìm thấy mã lớp tín chỉ {maltc}.",
                    Data = null
                });
            }

            if (course.SisoHientai > 0)
            {
                return NotFound(new ApiResponse<Loptinchi>
                {
                    Message = $"Lớp tín chỉ {maltc} đã được sinh viên đăng ký , không thể xóa.",
                    Data = null
                });
            }

            _context.Loptinchis.Remove(course);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<Loptinchi>
            {
                Message = $"Xóa lơp tín chỉ {maltc} thành công.",
                Data = null
            });
        }

        [HttpPut("update-open-course/{maltc}")]
        public async Task<IActionResult> UpdateOpenCourse(int maltc, OpenCourseDTO openCourseDTO)
        {
            var course = await _context.Loptinchis.FirstOrDefaultAsync(x => x.Maltc == maltc);
            if (course == null)
            {
                return Ok(new ApiResponse<Loptinchi>
                {
                    Message = $"Không tìm thấy lớp tín chỉ {maltc}.",
                    Data = null
                });
            }

            course.Nienkhoa = openCourseDTO.Nienkhoa;
            course.Hocky = openCourseDTO.Hocky;
            course.Mamh = openCourseDTO.Mamh;
            course.Magv = openCourseDTO.Magv;
            course.SisoToida = openCourseDTO.SisoToida;
            course.SisoHientai = openCourseDTO.SisoHientai;
            course.DayThutrongtuan = openCourseDTO.DayThutrongtuan;
            course.ThoigianBatdau = openCourseDTO.ThoigianBatdau;
            course.ThoigianKetthuc = openCourseDTO.ThoigianKetthuc;
            course.Huylop = openCourseDTO.Huylop;

            await _context.SaveChangesAsync();
            return Ok(new ApiResponse<Loptinchi>
            {
                Message = $"Cập nhật thông tin lớp tín chỉ {maltc} thành công.",
                Data = course
            });
        }

        [HttpPost("create-open-course")]
        public async Task<IActionResult> CreateOpenCourse(OpenCourseDTO opencourseDTO)
        {
            var newCourseSection = new Models.Loptinchi
            {
                Nienkhoa = opencourseDTO.Nienkhoa,
                Hocky = opencourseDTO.Hocky,
                Mamh = opencourseDTO.Mamh,
                Magv = opencourseDTO.Magv,
                SisoToida = opencourseDTO.SisoToida,
                SisoHientai = opencourseDTO.SisoHientai,
                DayThutrongtuan = opencourseDTO.DayThutrongtuan,
                ThoigianBatdau = opencourseDTO.ThoigianBatdau,
                ThoigianKetthuc = opencourseDTO.ThoigianKetthuc,
                Huylop = opencourseDTO.Huylop
            };

            _context.Loptinchis.Add(newCourseSection);
            await _context.SaveChangesAsync();
            return Ok(new ApiResponse<Loptinchi>
            {
                Message = $"Tạo mới lớp tín chỉ thành công.",
                Data = newCourseSection
            });
        }
    }
}
