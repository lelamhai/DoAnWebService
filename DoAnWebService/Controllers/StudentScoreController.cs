using DoAnWebService.DTO.Student;
using DoAnWebService.Utils;
using DoAnWebService.Utlis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DoAnWebService.Controllers
{
    [Route("api/v1/private/[controller]")]
    [ApiController]
    public class StudentScoreController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public StudentScoreController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("list")]
        public async Task<IActionResult> List(string masv, int page)
        {
            List<PointStudent> list = new();
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand cmd = new SqlCommand("SP_LAYDIEM_SV", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MASV", masv);
                    await conn.OpenAsync();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            list.Add(new PointStudent
                            {
                                MaMH = reader["MAMH"] == DBNull.Value
                                ? null
                                : reader["MAMH"].ToString(),

                                TenMH = reader["TENMH"] == DBNull.Value
                                ? null
                                : reader["TENMH"].ToString(),

                                SoTinChi = reader["SOTINCHI"] == DBNull.Value
                                ? 0
                                : Convert.ToInt32(reader["SOTINCHI"]),

                                NienKhoa = reader["NIENKHOA"] == DBNull.Value
                                ? null
                                : reader["NIENKHOA"].ToString(),

                                HocKy = reader["HOCKY"] == DBNull.Value
                                ? 0
                                : Convert.ToInt32(reader["HOCKY"]),

                                DiemCC = reader["DIEM_CC"] == DBNull.Value
                                ? null
                                : Convert.ToDecimal(reader["DIEM_CC"]),

                                DiemGK = reader["DIEM_GK"] == DBNull.Value
                                ? null
                                : Convert.ToDecimal(reader["DIEM_GK"]),

                                DiemCK = reader["DIEM_CK"] == DBNull.Value
                                ? null
                                : Convert.ToDecimal(reader["DIEM_CK"]),

                                DiemTong = reader["DIEM_TONG"] == DBNull.Value
                                ? null
                                : Convert.ToDecimal(reader["DIEM_TONG"])
                            });
                        }
                    }
                }
            }

            var result = PaginationHelper.CreatePagedResult(list, page, -1);
            return Ok(new APIResponse<PagedResult<PointStudent>>
            {
                Message = "Lấy danh sách điểm sinh viên thành công.",
                Data = result
            });
        }
    }
}
