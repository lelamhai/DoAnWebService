using Azure.Core;
using DoAnWebService.Data;
using DoAnWebService.Models;
using DoAnWebService.Utils;
using DoAnWebService.Utlis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAnWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly QLSVContext _context;

        public StudentsController(QLSVContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetStudents(int page = 1)
        {
            var students = await _context.Sinhviens.ToListAsync();
            var result = PaginationHelper.CreatePagedResult(students, page);
            return Ok(new ApiResponse<PagedResult<Sinhvien>>
            {
                StatusCode = 200,
                Success = true,
                Message = "Lấy danh sách sinh viên thành công.",
                Data = result
            });
        }

        [HttpGet("{masv}")]
        public async Task<ActionResult<Sinhvien>> GetStudentByMaSV(string masv)
        {
            var student = await _context.Sinhviens.FirstOrDefaultAsync(sv => sv.Masv == masv);

            if (student == null)
            {
                return Ok(new ApiResponse<Sinhvien>
                {
                    StatusCode = 404,
                    Success = true,
                    Message = "Không tìm thấy sinh viên.",
                    Data = null
                });
            }

            return Ok(new ApiResponse<Sinhvien>
            {
                StatusCode = 200,
                Success = true,
                Message = "Tìm sinh viên thành công",
                Data = student
            });
        }

        [HttpDelete("{masv}")]
        public async Task<IActionResult> DeleteStudent(string masv)
        {
            var student = await _context.Sinhviens.FirstOrDefaultAsync(x => x.Masv == masv);

            if (student == null)
            {
                return Ok(new ApiResponse<Sinhvien>
                {
                    StatusCode = 404,
                    Success = true,
                    Message = "Không tìm thấy sinh viên.",
                    Data = null
                });
            }

            if (await _context.Dangkies.AnyAsync(x => x.Masv == masv))
            {
                return Ok(new ApiResponse<Sinhvien>
                {
                    StatusCode = 404,
                    Success = true,
                    Message = "Sinh viên đã đăng ký lớp tín chỉ, không thể xóa.",
                    Data = null
                });
            }

            if (await _context.Hocphis.AnyAsync(x => x.Masv == masv))
            {
                return Ok(new ApiResponse<Sinhvien>
                {
                    StatusCode = 404,
                    Success = true,
                    Message = "Sinh viên đã có học phí, không thể xóa.",
                    Data = null
                });
            }

            _context.Sinhviens.Remove(student);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<Sinhvien>
            {
                StatusCode = 200,
                Success = true,
                Message = "Xóa sinh viên thành công.",
                Data = null
            });
        }
    }
}
