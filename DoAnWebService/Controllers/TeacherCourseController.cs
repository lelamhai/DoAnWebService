using DoAnWebService.DTO.Student;
using DoAnWebService.Utils;
using DoAnWebService.Utlis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DoAnWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherCourseController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public TeacherCourseController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("get-teacher-subject")]
        public async Task<IActionResult> GetTeacherSubject(
    [FromQuery] string maGv,
    [FromQuery] int page = 1)
        {
            if (string.IsNullOrWhiteSpace(maGv))
            {
                return BadRequest(new APIResponse<object>
                {
                    Message = "Mã giảng viên không được để trống.",
                    Data = null
                });
            }

            if (page < 1)
            {
                page = 1;
            }

            List<LTCModel> list = new();

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
                    new SqlCommand("SP_LAYDS_LTC_GV", conn);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@MAGV", SqlDbType.VarChar, 20).Value =
                    maGv.Trim();

                await conn.OpenAsync();

                using SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    list.Add(new LTCModel
                    {
                        MaLtc = reader["MALTC"] == DBNull.Value
                            ? 0
                            : Convert.ToInt32(reader["MALTC"]),

                        NienKhoa = reader["NIENKHOA"] == DBNull.Value
                            ? string.Empty
                            : reader["NIENKHOA"].ToString()!,

                        HocKy = reader["HOCKY"] == DBNull.Value
                            ? 0
                            : Convert.ToInt32(reader["HOCKY"]),

                        MaMh = reader["MAMH"] == DBNull.Value
                            ? string.Empty
                            : reader["MAMH"].ToString()!,

                        TenMh = reader["TENMH"] == DBNull.Value
                            ? string.Empty
                            : reader["TENMH"].ToString()!,

                        SoTinChi = reader["SOTINCHI"] == DBNull.Value
                            ? 0
                            : Convert.ToInt32(reader["SOTINCHI"]),

                        SoTietLt = reader["SOTIET_LT"] == DBNull.Value
                            ? 0
                            : Convert.ToInt32(reader["SOTIET_LT"]),

                        SoTietTh = reader["SOTIET_TH"] == DBNull.Value
                            ? 0
                            : Convert.ToInt32(reader["SOTIET_TH"]),

                        MaGv = reader["MAGV"] == DBNull.Value
                            ? null
                            : reader["MAGV"].ToString(),

                        TenGiangVien = reader["TENGIANGVIEN"] == DBNull.Value
                            ? null
                            : reader["TENGIANGVIEN"].ToString(),

                        SiSoHienTai = reader["SISO_HIENTAI"] == DBNull.Value
                            ? 0
                            : Convert.ToInt32(reader["SISO_HIENTAI"]),

                        SiSoToiDa = reader["SISO_TOIDA"] == DBNull.Value
                            ? 0
                            : Convert.ToInt32(reader["SISO_TOIDA"]),

                        SiSo = reader["SISO"] == DBNull.Value
                            ? string.Empty
                            : reader["SISO"].ToString()!,

                        DayThuTrongTuan =
                            reader["DAY_THUTRONGTUAN"] == DBNull.Value
                                ? string.Empty
                                : reader["DAY_THUTRONGTUAN"].ToString()!,

                        LichHoc = reader["LICHHOC"] == DBNull.Value
                            ? string.Empty
                            : reader["LICHHOC"].ToString()!,

                        ThoiGianBatDau =
                            reader["THOIGIAN_BATDAU"] == DBNull.Value
                                ? null
                                : Convert.ToDateTime(
                                    reader["THOIGIAN_BATDAU"]),

                        ThoiGianKetThuc =
                            reader["THOIGIAN_KETTHUC"] == DBNull.Value
                                ? null
                                : Convert.ToDateTime(
                                    reader["THOIGIAN_KETTHUC"]),

                        ThoiGianHoc = reader["THOIGIAN_HOC"] == DBNull.Value
                            ? string.Empty
                            : reader["THOIGIAN_HOC"].ToString()!,

                        HuyLop = reader["HUYLOP"] != DBNull.Value
                                 && Convert.ToInt32(reader["HUYLOP"]) == 1
                    });
                }

                var result = PaginationHelper.CreatePagedResult(
                    list,
                    page,
                    10
                );

                return Ok(new APIResponse<PagedResult<LTCModel>>
                {
                    Message = list.Count > 0
                        ? "Lấy danh sách môn giảng dạy thành công."
                        : "Giảng viên chưa được phân công lớp học phần nào.",
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
