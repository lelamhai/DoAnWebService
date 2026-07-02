using DoAnWebService.DTO.Student;
using DoAnWebService.Utils;
using DoAnWebService.Utlis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DoAnWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LTCController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public LTCController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("get-ltc")]
        public async Task<IActionResult> GetLopTinChi(int page = 1)
        {
            List<LTCModel> list = new();

            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand cmd = new SqlCommand("SP_LAYDS_LTC", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    await conn.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
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
                    }
                }
            }

            var result = PaginationHelper.CreatePagedResult(list, page, -1);

            return Ok(new APIResponse<PagedResult<LTCModel>>
            {
                Message = "Lấy danh sách lớp tín chỉ thành công.",
                Data = result
            });
        }
        
    }
}
