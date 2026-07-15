using DoAnWebService.DTO.Employment;
using DoAnWebService.DTO.Student;
using DoAnWebService.DTO.Teacher;
using DoAnWebService.Utils;
using DoAnWebService.Utlis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Globalization;

namespace DoAnWebService.Controllers
{
    [Route("api/v1/private/[controller]")]
    [ApiController]
    public class CourseSectionController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public CourseSectionController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("get-ltc")]
        public async Task<IActionResult> GetLopTinChi(int page = 1)
        {
            List<LTC_NVModel> list = new();

            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand cmd = new SqlCommand("SP_LAYDS_LTC_NV", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

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

                                MaLopHp = reader["MALOPHP"] == DBNull.Value
                                    ? string.Empty
                                    : reader["MALOPHP"].ToString()!,

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

                                SiSo = reader["SISO"] == DBNull.Value
                                    ? string.Empty
                                    : reader["SISO"].ToString()!,

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
                Message = "Lấy danh sách lớp tín chỉ thành công.",
                Data = result
            });
        }

        [HttpGet("get-subject")]
        public async Task<IActionResult> GetMonHoc()
        {
            List<SubjectModel> list = new();
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand cmd = new SqlCommand("SP_LAYDS_MH", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    await conn.OpenAsync();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            list.Add(new SubjectModel
                            {
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
                                    : Convert.ToInt32(reader["SOTIET_TH"])
                            });
                        }
                    }
                }
            }
            var result = PaginationHelper.CreatePagedResult(list, -1, -1);

            return Ok(new APIResponse<PagedResult<SubjectModel>>
            {
                Message = "Lấy danh sách lớp tín chỉ thành công.",
                Data = result
            });
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchTeachers(string? keyword)
        {
            return Ok();
        }

        [HttpDelete("delete/{ltc}")]
        public async Task<IActionResult> Delete(int ltc)
        {
           return Ok();
        }

        [HttpGet("detail/{ltc}")]
        public async Task<IActionResult> Detail(int ltc)
        {
            return Ok();
        }

        [HttpPut("update/{ltc}")]
        public async Task<IActionResult> Update(string ltc, UpdateTeacherModel model)
        {
            return Ok();
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateLTCModel model)
        {
            try
            {
                if (!DateTime.TryParseExact(
                    model.ThoiGianBatDau,
                    new[] { "dd/MM/yyyy", "yyyy-MM-dd" },
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime ngayThoiGianBatDau))
                {
                    return BadRequest(new
                    {
                        message = "Ngày sinh không đúng định dạng. " +
                                  "Vui lòng nhập dd/MM/yyyy hoặc yyyy-MM-dd."
                    });
                }

                if (!DateTime.TryParseExact(
                   model.ThoiGianKetThuc,
                   new[] { "dd/MM/yyyy", "yyyy-MM-dd" },
                   CultureInfo.InvariantCulture,
                   DateTimeStyles.None,
                   out DateTime ngayThoiGianKetThuc))
                {
                    return BadRequest(new
                    {
                        message = "Ngày sinh không đúng định dạng. " +
                                  "Vui lòng nhập dd/MM/yyyy hoặc yyyy-MM-dd."
                    });
                }

                using var conn = new SqlConnection(
                    _configuration.GetConnectionString("DefaultConnection")
                );
                using var cmd = new SqlCommand("SP_THEM_LTC", conn);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@NIENKHOA", SqlDbType.NChar, 9)
                    .Value = model.NienKhoa.Trim();

                cmd.Parameters.Add("@HOCKY", SqlDbType.Int)
                    .Value = model.HocKy;

                cmd.Parameters.Add("@MAMH", SqlDbType.NChar, 10)
                    .Value = model.MaMH.Trim();

                cmd.Parameters.Add("@MAGV", SqlDbType.NChar, 10)
                    .Value = model.MaGV.Trim();

                cmd.Parameters.Add("@SISO_TOIDA", SqlDbType.Int)
                    .Value = model.SiSoToiDa;

                cmd.Parameters.Add("@DAY_THUTRONGTUAN", SqlDbType.VarChar, 50)
                    .Value = string.IsNullOrWhiteSpace(model.DayThuTrongTuan)
                        ? DBNull.Value
                        : model.DayThuTrongTuan.Trim();
                
                cmd.Parameters.Add("@THOIGIAN_BATDAU", SqlDbType.Date)
                    .Value = ngayThoiGianBatDau;

                cmd.Parameters.Add("@THOIGIAN_KETTHUC", SqlDbType.Date)
                    .Value = ngayThoiGianKetThuc;

                cmd.Parameters.Add("@HUYLOP", SqlDbType.Bit)
                    .Value = model.HuyLop;

                var messageParam = new SqlParameter(
                    "@MESSAGE",
                    SqlDbType.NVarChar,
                    200
                )
                {
                    Direction = ParameterDirection.Output
                };

                cmd.Parameters.Add(messageParam);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                string message = messageParam.Value?.ToString() ?? "0";

                if (message == "0")
                {
                    return NotFound(new APIResponse<object>
                    {
                        Message = "Không tìm thấy giảng viên",
                        Data = null
                    });
                }

                return Ok(new APIResponse<object>
                {
                    Message = "Thêm mới LTC thành công",
                    Data = null
                });
            }
            catch (SqlException ex)
            {
                return BadRequest(new
                {
                    message = "Lỗi cơ sở dữ liệu",
                    error = ex.Message
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = "Lỗi hệ thống",
                    error = ex.Message
                });
            }
        }

    }
}
