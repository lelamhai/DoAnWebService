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
    public class RegisterLTCController : ControllerBase
    {
        //private readonly IConfiguration _configuration;

        //public RegisterLTCController(IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //}

        //[HttpGet("get-register-ltc")]
        //public async Task<IActionResult> GetLopTinChi( string maSv, int page = 1)
        //{
        //    if (string.IsNullOrWhiteSpace(maSv))
        //    {
        //        return BadRequest(new APIResponse<object>
        //        {
        //            Message = "Mã sinh viên không được để trống.",
        //            Data = null
        //        });
        //    }

        //    if (page < 1)
        //    {
        //        page = 1;
        //    }

        //    List<RegisterLTCModel> list = new();

        //    try
        //    {
        //        string? connectionString =
        //            _configuration.GetConnectionString("DefaultConnection");

        //        using SqlConnection conn = new SqlConnection(connectionString);

        //        using SqlCommand cmd =
        //            new SqlCommand("SP_LAYDS_DKLTC", conn);

        //        cmd.CommandType = CommandType.StoredProcedure;

        //        cmd.Parameters.Add("@MASV", SqlDbType.VarChar, 20).Value =
        //            maSv.Trim();

        //        await conn.OpenAsync();

        //        using SqlDataReader reader = await cmd.ExecuteReaderAsync();

        //        while (await reader.ReadAsync())
        //        {
        //            list.Add(new RegisterLTCModel
        //            {
        //                MaLtc = reader["MALTC"] == DBNull.Value
        //                    ? 0
        //                    : Convert.ToInt32(reader["MALTC"]),

        //                MaMh = reader["MAMH"] == DBNull.Value
        //                    ? string.Empty
        //                    : reader["MAMH"].ToString()!,

        //                TenMh = reader["TENMH"] == DBNull.Value
        //                    ? string.Empty
        //                    : reader["TENMH"].ToString()!,

        //                SoTinChi = reader["SOTINCHI"] == DBNull.Value
        //                    ? 0
        //                    : Convert.ToInt32(reader["SOTINCHI"]),

        //                MaGv = reader["MAGV"] == DBNull.Value
        //                    ? null
        //                    : reader["MAGV"].ToString(),

        //                TenGiangVien = reader["TENGIANGVIEN"] == DBNull.Value
        //                    ? null
        //                    : reader["TENGIANGVIEN"].ToString(),

        //                NienKhoa = reader["NIENKHOA"] == DBNull.Value
        //                    ? string.Empty
        //                    : reader["NIENKHOA"].ToString()!,

        //                HocKy = reader["HOCKY"] == DBNull.Value
        //                    ? 0
        //                    : Convert.ToInt32(reader["HOCKY"]),

        //                DayThuTrongTuan =
        //                    reader["DAY_THUTRONGTUAN"] == DBNull.Value
        //                        ? string.Empty
        //                        : reader["DAY_THUTRONGTUAN"].ToString()!,

        //                LichHoc = reader["LICHHOC"] == DBNull.Value
        //                    ? string.Empty
        //                    : reader["LICHHOC"].ToString()!,

        //                ThoiGianBatDau =
        //                    reader["THOIGIAN_BATDAU"] == DBNull.Value
        //                        ? null
        //                        : Convert.ToDateTime(
        //                            reader["THOIGIAN_BATDAU"]),

        //                ThoiGianKetThuc =
        //                    reader["THOIGIAN_KETTHUC"] == DBNull.Value
        //                        ? null
        //                        : Convert.ToDateTime(
        //                            reader["THOIGIAN_KETTHUC"]),

        //                ThoiGianHoc = reader["THOIGIAN_HOC"] == DBNull.Value
        //                    ? string.Empty
        //                    : reader["THOIGIAN_HOC"].ToString()!
        //            });
        //        }

        //        var result = PaginationHelper.CreatePagedResult(
        //            list,
        //            page,
        //            10
        //        );

        //        return Ok(new APIResponse<PagedResult<RegisterLTCModel>>
        //        {
        //            Message = list.Count > 0
        //                ? "Lấy danh sách lớp học phần đã đăng ký thành công."
        //                : "Sinh viên chưa đăng ký lớp học phần nào.",
        //            Data = result
        //        });
        //    }
        //    catch (SqlException ex)
        //    {
        //        return StatusCode(
        //            StatusCodes.Status500InternalServerError,
        //            new APIResponse<object>
        //            {
        //                Message = $"Lỗi cơ sở dữ liệu: {ex.Message}",
        //                Data = null
        //            }
        //        );
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(
        //            StatusCodes.Status500InternalServerError,
        //            new APIResponse<object>
        //            {
        //                Message = $"Đã xảy ra lỗi: {ex.Message}",
        //                Data = null
        //            }
        //        );
        //    }
        //}
    }
}
