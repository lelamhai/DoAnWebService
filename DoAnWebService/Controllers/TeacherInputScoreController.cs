using DoAnWebService.DTO.Student;
using DoAnWebService.DTO.Teacher;
using DoAnWebService.Utils;
using DoAnWebService.Utlis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DoAnWebService.Controllers
{
    [Route("api/v1/private/[controller]")]
    [ApiController]
    [Authorize(Roles = "GV")]
    public class TeacherInputScoreController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public TeacherInputScoreController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpGet("get-teacher-inputscore")]
        public async Task<IActionResult> GetInputScore(string maGv, int page = 1)
        {
            if (string.IsNullOrWhiteSpace(maGv))
            {
                return BadRequest(new APIResponse<object>
                {
                    Message = "Mã giảng viên không được để trống.",
                    Data = null
                });
            }

           
            List<TeacherInputScoreModel> list = new();

            try
            {
                string? connectionString =
                    _configuration.GetConnectionString("DefaultConnection");

                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    return StatusCode(
                        StatusCodes.Status500InternalServerError,
                        new APIResponse<object>
                        {
                            Message = "Không tìm thấy chuỗi kết nối cơ sở dữ liệu.",
                            Data = null
                        });
                }

                using SqlConnection conn = new SqlConnection(connectionString);

                using SqlCommand cmd =
                    new SqlCommand("SP_LAYDS_DIEM_THEO_GV", conn);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@MAGV", SqlDbType.VarChar, 20).Value =
                    maGv.Trim();

                await conn.OpenAsync();

                using SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    list.Add(new TeacherInputScoreModel
                    {
                        Stt = reader["STT"] == DBNull.Value
                            ? 0
                            : Convert.ToInt64(reader["STT"]),

                        MaLtc = reader["MALTC"] == DBNull.Value
                            ? 0
                            : Convert.ToInt32(reader["MALTC"]),

                        MaSv = reader["MASV"] == DBNull.Value
                            ? string.Empty
                            : reader["MASV"].ToString()!,

                        MaMh = reader["MAMH"] == DBNull.Value
                            ? string.Empty
                            : reader["MAMH"].ToString()!,

                        TenMh = reader["TENMH"] == DBNull.Value
                            ? string.Empty
                            : reader["TENMH"].ToString()!,

                        SoTinChi = reader["SOTINCHI"] == DBNull.Value
                            ? 0
                            : Convert.ToInt32(reader["SOTINCHI"]),

                        HocKy = reader["HOCKY"] == DBNull.Value
                            ? 0
                            : Convert.ToInt32(reader["HOCKY"]),

                        NienKhoa = reader["NIENKHOA"] == DBNull.Value
                            ? string.Empty
                            : reader["NIENKHOA"].ToString()!,

                        DiemCc = reader["DIEM_CC"] == DBNull.Value
                            ? null
                            : Convert.ToDecimal(reader["DIEM_CC"]),

                        DiemGk = reader["DIEM_GK"] == DBNull.Value
                            ? null
                            : Convert.ToDecimal(reader["DIEM_GK"]),

                        DiemCk = reader["DIEM_CK"] == DBNull.Value
                            ? null
                            : Convert.ToDecimal(reader["DIEM_CK"]),

                        DiemTong = reader["DIEM_TONG"] == DBNull.Value
                            ? null
                            : Convert.ToDecimal(reader["DIEM_TONG"]),

                        XepLoai = reader["XEPLOAI"] == DBNull.Value
                            ? string.Empty
                            : reader["XEPLOAI"].ToString()!
                    });
                }

                var result = PaginationHelper.CreatePagedResult(
                    list,
                    page,
                    10
                );

                return Ok(new APIResponse<PagedResult<TeacherInputScoreModel>>
                {
                    Message = list.Count > 0
                        ? "Lấy danh sách điểm theo giảng viên thành công."
                        : "Không tìm thấy dữ liệu điểm của giảng viên.",
                    Data = result
                });
            }
            catch (SqlException ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new APIResponse<object>
                    {
                        Message = $"Lỗi cơ sở dữ liệu: {ex.Message}",
                        Data = null
                    });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new APIResponse<object>
                    {
                        Message = $"Đã xảy ra lỗi: {ex.Message}",
                        Data = null
                    });
            }
        }
    }
}
