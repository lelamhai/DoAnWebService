using DoAnWebService.DTO.Student;
using DoAnWebService.Utlis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DoAnWebService.Controllers
{
    [Route("api/v1/private/[controller]")]
    [ApiController]
    public class StudentTuitionFeesController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public StudentTuitionFeesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("list")]
        public async Task<IActionResult> List(string maSv)
        {
            if (string.IsNullOrWhiteSpace(maSv))
            {
                return BadRequest(new APIResponse<object>
                {
                    Message = "Mã sinh viên không được để trống.",
                    Data = null
                });
            }


            List<HocPhiModel> list = new();


            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand cmd = new SqlCommand("SP_HOCPHI", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.Add("@MASV", SqlDbType.VarChar, 20)
                        .Value = maSv.Trim();


                    await conn.OpenAsync();


                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            list.Add(new HocPhiModel
                            {
                                MaSv = reader["MASV"] == DBNull.Value
                                    ? string.Empty
                                    : reader["MASV"].ToString()!,


                                NienKhoa = reader["NIENKHOA"] == DBNull.Value
                                    ? string.Empty
                                    : reader["NIENKHOA"].ToString()!,


                                HocKy = reader["HOCKY"] == DBNull.Value
                                    ? 0
                                    : Convert.ToInt32(reader["HOCKY"]),


                                SoMonHoc = reader["SOMONHOC"] == DBNull.Value
                                    ? 0
                                    : Convert.ToInt32(reader["SOMONHOC"]),


                                TongSoTinChi = reader["TONGSOTINCHI"] == DBNull.Value
                                    ? 0
                                    : Convert.ToInt32(reader["TONGSOTINCHI"]),


                                TongHocPhi = reader["TONGHOCPHI"] == DBNull.Value
                                    ? 0
                                    : Convert.ToInt64(reader["TONGHOCPHI"])
                            });
                        }
                    }
                }
            }


            if (list.Count == 0)
            {
                return NotFound(new APIResponse<object>
                {
                    Message = $"Không tìm thấy học phí của sinh viên {maSv}.",
                    Data = null
                });
            }


            return Ok(new APIResponse<List<HocPhiModel>>
            {
                Message = "Lấy thông tin học phí thành công.",
                Data = list
            });
        }
    }
}
