using DoAnWebService.Data;
using DoAnWebService.DTO.Classroom;
using DoAnWebService.DTO.Lop;
using DoAnWebService.Models;
using DoAnWebService.Utils;
using DoAnWebService.Utlis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DoAnWebService.Controllers
{
    [Route("api/v1/private/[controller]")]
    [ApiController]
    [Authorize]
    public class ClassroomController : ControllerBase
    {
        
        private readonly IConfiguration _configuration;

        public ClassroomController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("get-classrooms")]
        public async Task<IActionResult> GetClassrooms(int page = 1)
        {
            List<ClassroomModel> list = new();

            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand cmd = new SqlCommand("SP_LAYLOP", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    await conn.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            list.Add(new ClassroomModel
                            {
                                Malop = reader["MALOP"].ToString(),
                                Tenlop = reader["TENLOP"].ToString(),
                                Khoahoc = reader["KHOAHOC"].ToString(),
                                Tenkhoa = reader["TENKHOA"].ToString(),
                                Tennv = reader["HOTENNV"].ToString(),
                                Ngaymolop = Convert.ToDateTime(reader["NGAYMOLOP"]).ToString("dd/MM/yyyy"),
                                Trangthai = reader["TENTRANGTHAI"].ToString()
                            });
                        }
                    }
                }
            }
            var result = PaginationHelper.CreatePagedResult(list, page, -1);
            return Ok(new ApiResponse<PagedResult<ClassroomModel>>
            {
                Message = "Lấy danh sách lớp thành công.",
                Data = result
            });
        }

        //[HttpGet("detail-classroom/{malop}")]
        //public async Task<ActionResult<Lop>> GetLopByMaLop(string malop)
        //{
        //    var classroom = await _context.Lops.FirstOrDefaultAsync(l => l.Malop == malop);

        //    if (classroom == null)
        //    {
        //        return NotFound(new ApiResponse<Lop>
        //        {
        //            Message = $"Không tìm thấy lớp {malop}.",
        //            Data = null
        //        });
        //    }

        //    return Ok(new ApiResponse<Lop>
        //    {
        //        Message = $"Tìm lớp {malop} thành công.",
        //        Data = classroom
        //    });
        //}

        //[HttpDelete("delete-classroom/{malop}")]
        //public async Task<IActionResult> DeleteStudent(string malop)
        //{
        //    var classroom = await _context.Lops.FirstOrDefaultAsync(x => x.Malop == malop);

        //    if (classroom == null)
        //    {
        //        return NotFound(new ApiResponse<Lop>
        //        {
        //            Message = $"Không tìm thấy lớp {malop}.",
        //            Data = null
        //        });
        //    }

        //    if (await _context.Sinhviens.AnyAsync(x => x.Malop == malop))
        //    {
        //        return NotFound(new ApiResponse<Sinhvien>
        //        {
        //            Message = $"Lớp {malop} đã có sinh viên đăng ký, không thể xóa.",
        //            Data = null
        //        });
        //    }
            

        //    _context.Lops.Remove(classroom);
        //    await _context.SaveChangesAsync();

        //    return Ok(new ApiResponse<Lop>
        //    {
        //        Message = $"Xóa lớp {malop} thành công.",
        //        Data = null
        //    });
        //}

        //[HttpPut("update-classroom/{malop}")]
        //public async Task<IActionResult> UpdateStudent(string malop, BaseClassroomDTO classroomDTO)
        //{
        //    var classroom = await _context.Lops.FirstOrDefaultAsync(x => x.Malop == malop);
        //    if (classroom == null)
        //    {
        //        return Ok(new ApiResponse<Lop>
        //        {
        //            Message = $"Không tìm thấy lớp {malop}.",
        //            Data = null
        //        });
        //    }
        //    classroom.Tenlop=classroomDTO.Tenlop;
        //    classroom.Khoahoc=classroomDTO.Khoahoc;
        //    classroom.Makhoa=classroomDTO.Makhoa;
        //    classroom.Manv=classroomDTO.Manv;


        //    await _context.SaveChangesAsync();
        //    return Ok(new ApiResponse<Lop>
        //    {
        //        Message = $"Cập nhật thông tin lớp {malop} thành công.",
        //        Data = classroom
        //    });
        //}

        //[HttpPost("create-classroom")]
        //public async Task<IActionResult> CreateClassroom(CreateClassroomDTO classroomDTO)
        //{
        //    var newClassroom = new Lop
        //    {
        //        Malop = classroomDTO.Malop,
        //        Tenlop = classroomDTO.Tenlop,
        //        Khoahoc = classroomDTO.Khoahoc,
        //        Makhoa = classroomDTO.Makhoa,
        //        Manv = classroomDTO.Manv
        //    };
        //    _context.Lops.Add(newClassroom);
        //    await _context.SaveChangesAsync();
        //    return Ok(new ApiResponse<Lop>
        //    {
        //        Message = $"Tạo mới lớp {classroomDTO.Malop} thành công.",
        //        Data = newClassroom
        //    });
        //}
    }
}
