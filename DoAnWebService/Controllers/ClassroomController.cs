using DoAnWebService.Data;
using DoAnWebService.DTO.Lop;
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
    public class ClassroomController : ControllerBase
    {
        private readonly QLSVContext _context;

        public ClassroomController(QLSVContext context)
        {
            _context = context;
        }

        [HttpGet("get-classrooms")]
        public async Task<IActionResult> GetClassrooms(int page = 1)
        {
            var classrooms = await _context.Lops.ToListAsync();
            var result = PaginationHelper.CreatePagedResult(classrooms, page, -1);
            return Ok(new ApiResponse<PagedResult<Lop>>
            {
                Message = "Lấy danh sách lớp thành công.",
                Data = result
            });
        }

        [HttpGet("detail-classroom/{malop}")]
        public async Task<ActionResult<Lop>> GetLopByMaLop(string malop)
        {
            var classroom = await _context.Lops.FirstOrDefaultAsync(l => l.Malop == malop);

            if (classroom == null)
            {
                return NotFound(new ApiResponse<Lop>
                {
                    Message = $"Không tìm thấy lớp {malop}.",
                    Data = null
                });
            }

            return Ok(new ApiResponse<Lop>
            {
                Message = $"Tìm lớp {malop} thành công.",
                Data = classroom
            });
        }

        [HttpDelete("delete-classroom/{malop}")]
        public async Task<IActionResult> DeleteStudent(string malop)
        {
            var classroom = await _context.Lops.FirstOrDefaultAsync(x => x.Malop == malop);

            if (classroom == null)
            {
                return NotFound(new ApiResponse<Lop>
                {
                    Message = $"Không tìm thấy lớp {malop}.",
                    Data = null
                });
            }

            if (await _context.Sinhviens.AnyAsync(x => x.Malop == malop))
            {
                return Ok(new ApiResponse<Sinhvien>
                {
                    Message = $"Lớp {malop} đã có sinh viên đăng ký, không thể xóa.",
                    Data = null
                });
            }
            

            _context.Lops.Remove(classroom);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<Sinhvien>
            {
                Message = $"Xóa lớp {malop} thành công.",
                Data = null
            });
        }

        [HttpPut("update-classroom/{malop}")]
        public async Task<IActionResult> UpdateStudent(string malop, BaseClassroomDTO classroomDTO)
        {
            var classroom = await _context.Lops.FirstOrDefaultAsync(x => x.Malop == malop);
            if (classroom == null)
            {
                return Ok(new ApiResponse<Lop>
                {
                    Message = $"Không tìm thấy lớp {malop}.",
                    Data = null
                });
            }
            classroom.Tenlop=classroomDTO.Tenlop;
            classroom.Khoahoc=classroomDTO.Khoahoc;
            classroom.Makhoa=classroomDTO.Makhoa;
            classroom.Manv=classroomDTO.Manv;


            await _context.SaveChangesAsync();
            return Ok(new ApiResponse<Lop>
            {
                Message = $"Cập nhật thông tin lớp {malop} thành công.",
                Data = classroom
            });
        }

        [HttpPost("create-classroom")]
        public async Task<IActionResult> CreateClassroom(CreateClassroomDTO classroomDTO)
        {
            var newClassroom = new Lop
            {
                Malop = classroomDTO.Malop,
                Tenlop = classroomDTO.Tenlop,
                Khoahoc = classroomDTO.Khoahoc,
                Makhoa = classroomDTO.Makhoa,
                Manv = classroomDTO.Manv
            };
            _context.Lops.Add(newClassroom);
            await _context.SaveChangesAsync();
            return Ok(new ApiResponse<Lop>
            {
                Message = $"Tạo mới lớp {classroomDTO.Malop} thành công.",
                Data = newClassroom
            });
        }
    }
}
