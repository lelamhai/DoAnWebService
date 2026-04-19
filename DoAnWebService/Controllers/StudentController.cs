using DoAnWebService.Data;
using DoAnWebService.DTO.Sinhvien;
using DoAnWebService.Models;
using DoAnWebService.Utils;
using DoAnWebService.Utlis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAnWebService.Controllers
{
    [Route("api/v1/private/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly QLSVContext _context;

        public StudentController(QLSVContext context)
        {
            _context = context;
        }

        [HttpGet("get-students")]
        public async Task<IActionResult> GetStudents(int page = 1)
        {
            var students = await _context.Sinhviens.ToListAsync();
            var result = PaginationHelper.CreatePagedResult(students, page, 3);
            return Ok(new ApiResponse<PagedResult<Sinhvien>>
            {
                Message = "Lấy danh sách sinh viên thành công.",
                Data = result
            });
        }

        [HttpGet("detail-student/{masv}")]
        public async Task<ActionResult<Sinhvien>> GetStudentByMaSV(string masv)
        {
            var student = await _context.Sinhviens.FirstOrDefaultAsync(sv => sv.Masv == masv);

            if (student == null)
            {
                return NotFound(new ApiResponse<Sinhvien>
                {
                    Message = $"Không tìm thấy sinh viên {masv}.",
                    Data = null
                });
            }

            return Ok(new ApiResponse<Sinhvien>
            {
                Message = $"Tìm sinh viên {masv} thành công",
                Data = student
            });
        }

        [HttpDelete("delete-student/{masv}")]
        public async Task<IActionResult> DeleteStudent(string masv)
        {
            var student = await _context.Sinhviens.FirstOrDefaultAsync(x => x.Masv == masv);

            if (student == null)
            {
                return NotFound(new ApiResponse<Sinhvien>
                {
                    Message = $"Không tìm thấy sinh viên {masv}.",
                    Data = null
                });
            }

            if (await _context.Dangkies.AnyAsync(x => x.Masv == masv))
            {
                return Ok(new ApiResponse<Sinhvien>
                {
                    Message = $"Sinh viên {masv} đã đăng ký lớp tín chỉ, không thể xóa.",
                    Data = null
                });
            }

            if (await _context.Hocphis.AnyAsync(x => x.Masv == masv))
            {
                return Ok(new ApiResponse<Sinhvien>
                {
                    Message = $"Sinh viên {masv} đã có học phí, không thể xóa.",
                    Data = null
                });
            }

            _context.Sinhviens.Remove(student);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<Sinhvien>
            {
                Message = $"Xóa sinh viên {masv} thành công.",
                Data = null
            });
        }

        [HttpPut("update-student/{masv}")]
        public async Task<IActionResult> UpdateStudent(string masv, UpdateStudentDTO sinhvienDTO)
        {
            var student = await _context.Sinhviens.FirstOrDefaultAsync(x => x.Masv == masv);
            if (student == null)
            {
                return Ok(new ApiResponse<Sinhvien>
                {
                    Message = $"Không tìm thấy sinh viên {masv}.",
                    Data = null
                });
            }

            student.Masv = masv;
            student.Malop = sinhvienDTO.Malop;
            student.Ho = sinhvienDTO.Ho;
            student.Ten = sinhvienDTO.Ten;
            student.Phai = sinhvienDTO.Phai;
            student.Diachi = sinhvienDTO.Diachi;
            student.Ngaysinh = sinhvienDTO.Ngaysinh;
            student.Email = sinhvienDTO.Email;
            student.Password = sinhvienDTO.Password;
            student.Danghoc = sinhvienDTO.Danghoc;

            await _context.SaveChangesAsync();
            return Ok(new ApiResponse<Sinhvien>
            {
                Message = $"Cập nhật thông tin sinh viên {masv} thành công.",
                Data = student
            });
        }

        [HttpPost("create-student")]
        public async Task<IActionResult> CreateStudent(CreateStudentDTO sinhvienDTO)
        {
            var newStudent = new Sinhvien
            {
                Masv = sinhvienDTO.Masv,
                Malop = sinhvienDTO.Malop,
                Ho = sinhvienDTO.Ho,
                Ten = sinhvienDTO.Ten,
                Phai = sinhvienDTO.Phai,
                Diachi = sinhvienDTO.Diachi,
                Ngaysinh = sinhvienDTO.Ngaysinh,
                Email = sinhvienDTO.Email,
                Password = sinhvienDTO.Password,
                Danghoc = sinhvienDTO.Danghoc
            };
            _context.Sinhviens.Add(newStudent);
            await _context.SaveChangesAsync();
            return Ok(new ApiResponse<Sinhvien>
            {
                Message = $"Tạo mới sinh viên {sinhvienDTO.Masv} thành công.",
                Data = newStudent
            });
        }
    }
}
