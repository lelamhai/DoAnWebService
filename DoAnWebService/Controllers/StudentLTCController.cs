using DoAnWebService.DTO.Employment;
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
    public class StudentLTCController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public StudentLTCController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("get-ltc")]
        public async Task<IActionResult> GetLopTinChiTheoSinhVien(string maSv, int page = 1)
        {
            if (string.IsNullOrWhiteSpace(maSv))
            {
                return BadRequest(new APIResponse<object>
                {
                    Message = "Mã sinh viên không được để trống.",
                    Data = null
                });
            }

            List<LTC_NVModel> list = new();

            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand cmd = new SqlCommand("SP_LAYDS_LTC_SV", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@MASV", SqlDbType.VarChar, 20).Value = maSv.Trim();

                    await conn.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            list.Add(new LTC_NVModel
                            {
                                MaLtc = reader["MALTC"] == DBNull.Value
                                    ? 0
                                    : Convert.ToInt32(reader["MALTC"]),

                                MaLopHp = string.Empty,

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

                                HocKy = reader["HOCKY"] == DBNull.Value
                                    ? 0
                                    : Convert.ToInt32(reader["HOCKY"]),

                                NienKhoa = reader["NIENKHOA"] == DBNull.Value
                                    ? string.Empty
                                    : reader["NIENKHOA"].ToString()!,

                                SiSoHienTai = reader["SISO_HIENTAI"] == DBNull.Value
                                    ? 0
                                    : Convert.ToInt32(reader["SISO_HIENTAI"]),

                                SiSoToiDa = reader["SISO_TOIDA"] == DBNull.Value
                                    ? 0
                                    : Convert.ToInt32(reader["SISO_TOIDA"]),

                                // Nếu muốn hiển thị 29/50 thì nên dùng SISO_CHITIET
                                SiSo = reader["SISO_CHITIET"] == DBNull.Value
                                    ? string.Empty
                                    : reader["SISO_CHITIET"].ToString()!,

                                DayThuTrongTuan = reader["DAY_THUTRONGTUAN"] == DBNull.Value
                                    ? string.Empty
                                    : reader["DAY_THUTRONGTUAN"].ToString()!,

                                LichHoc = reader["LICHHOC"] == DBNull.Value
                                    ? string.Empty
                                    : reader["LICHHOC"].ToString()!,

                                ThoiGianBatDau = reader["THOIGIAN_BATDAU"] == DBNull.Value
                                    ? null
                                    : Convert.ToDateTime(reader["THOIGIAN_BATDAU"]),

                                ThoiGianKetThuc = reader["THOIGIAN_KETTHUC"] == DBNull.Value
                                    ? null
                                    : Convert.ToDateTime(reader["THOIGIAN_KETTHUC"]),

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

            return Ok(new APIResponse<PagedResult<LTC_NVModel>>
            {
                Message = "Lấy danh sách lớp tín chỉ theo sinh viên thành công.",
                Data = result
            });
        }
        
        [HttpGet("search")]
        public async Task<IActionResult> SearchLopTinChiTheoMonHoc(string maSv,string? keyword = "", int page = 1)
        {
            if (string.IsNullOrWhiteSpace(maSv))
            {
                return BadRequest(new APIResponse<object>
                {
                    Message = "Mã sinh viên không được để trống.",
                    Data = null
                });
            }

            List<LTC_NVModel> list = new();

            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand cmd = new SqlCommand("SP_TIMKIEM_LTC_SV", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@MASV", SqlDbType.VarChar, 20).Value = maSv.Trim();

                    cmd.Parameters.Add("@KEYWORD", SqlDbType.NVarChar, 100).Value =
                        string.IsNullOrWhiteSpace(keyword)
                            ? string.Empty
                            : keyword.Trim();

                    await conn.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            list.Add(new LTC_NVModel
                            {
                                MaLtc = reader["MALTC"] == DBNull.Value
                                    ? 0
                                    : Convert.ToInt32(reader["MALTC"]),

                                // Store proc hiện tại không SELECT MALOPHP
                                MaLopHp = string.Empty,

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

                                HocKy = reader["HOCKY"] == DBNull.Value
                                    ? 0
                                    : Convert.ToInt32(reader["HOCKY"]),

                                NienKhoa = reader["NIENKHOA"] == DBNull.Value
                                    ? string.Empty
                                    : reader["NIENKHOA"].ToString()!,

                                SiSoHienTai = reader["SISO_HIENTAI"] == DBNull.Value
                                    ? 0
                                    : Convert.ToInt32(reader["SISO_HIENTAI"]),

                                SiSoToiDa = reader["SISO_TOIDA"] == DBNull.Value
                                    ? 0
                                    : Convert.ToInt32(reader["SISO_TOIDA"]),

                                SiSo = reader["SISO_CHITIET"] == DBNull.Value
                                    ? string.Empty
                                    : reader["SISO_CHITIET"].ToString()!,

                                DayThuTrongTuan = reader["DAY_THUTRONGTUAN"] == DBNull.Value
                                    ? string.Empty
                                    : reader["DAY_THUTRONGTUAN"].ToString()!,

                                LichHoc = reader["LICHHOC"] == DBNull.Value
                                    ? string.Empty
                                    : reader["LICHHOC"].ToString()!,

                                ThoiGianBatDau = reader["THOIGIAN_BATDAU"] == DBNull.Value
                                    ? null
                                    : Convert.ToDateTime(reader["THOIGIAN_BATDAU"]),

                                ThoiGianKetThuc = reader["THOIGIAN_KETTHUC"] == DBNull.Value
                                    ? null
                                    : Convert.ToDateTime(reader["THOIGIAN_KETTHUC"]),

                                ThoiGianHoc = reader["THOIGIAN_HOC"] == DBNull.Value
                                    ? string.Empty
                                    : reader["THOIGIAN_HOC"].ToString()!,

                                HuyLop = reader["HUYLOP"] != DBNull.Value
                                         && Convert.ToBoolean(reader["HUYLOP"]),
                            });
                        }
                    }
                }
            }

            var result = PaginationHelper.CreatePagedResult(list, page, -1);

            return Ok(new APIResponse<PagedResult<LTC_NVModel>>
            {
                Message = "Tìm kiếm lớp tín chỉ theo môn học thành công.",
                Data = result
            });
        }


    }
}
