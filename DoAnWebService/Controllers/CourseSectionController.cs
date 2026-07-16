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

        //[HttpGet("search")]
        //public async Task<IActionResult> SearchTeachers(string? keyword)
        //{
        //    return Ok();
        //}

        [HttpDelete("delete/{maLtc}")]
        public async Task<IActionResult> Delete(int maLtc)
        {
            if (maLtc <= 0)
            {
                return BadRequest(new APIResponse<object>
                {
                    Message = "Mã lớp tín chỉ không hợp lệ.",
                    Data = null
                });
            }



            try
            {

                using (SqlConnection conn = new SqlConnection(
                    _configuration.GetConnectionString("DefaultConnection")))
                {

                    using (SqlCommand cmd = new SqlCommand(
                        "SP_XOA_LTC",
                        conn))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;


                        cmd.Parameters.Add("@MALTC",
                            SqlDbType.Int)
                            .Value = maLtc;



                        await conn.OpenAsync();



                        using (SqlDataReader reader =
                            await cmd.ExecuteReaderAsync())
                        {

                            if (await reader.ReadAsync())
                            {

                                return Ok(new APIResponse<object>
                                {
                                    Message = "Xóa lớp tín chỉ thành công.",
                                    Data = null
                                });

                            }

                        }

                    }

                }



                return BadRequest(new APIResponse<object>
                {
                    Message = "Không thể xóa lớp tín chỉ.",
                    Data = null
                });


            }
            catch (SqlException ex)
            {

                return BadRequest(new APIResponse<object>
                {
                    Message = ex.Message,
                    Data = null
                });

            }
            catch (Exception ex)
            {

                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new APIResponse<object>
                    {
                        Message = "Lỗi hệ thống: " + ex.Message,
                        Data = null
                    });

            }
        }

        [HttpGet("detail/{maLtc}")]
        public async Task<IActionResult> Detail(int maLtc)
        {

            if (maLtc <= 0)
            {
                return BadRequest(new APIResponse<object>
                {
                    Message = "Mã lớp tín chỉ không hợp lệ.",
                    Data = null
                });
            }



            ChiTietLTCModel? model = null;



            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection")))
            {

                using (SqlCommand cmd = new SqlCommand(
                    "SP_LAYMOT_LTC",
                    conn))
                {

                    cmd.CommandType = CommandType.StoredProcedure;



                    cmd.Parameters.Add("@MALTC",
                        SqlDbType.Int)
                        .Value = maLtc;



                    await conn.OpenAsync();



                    using (SqlDataReader reader =
                        await cmd.ExecuteReaderAsync())
                    {

                        if (await reader.ReadAsync())
                        {

                            model = new ChiTietLTCModel
                            {


                                MaLtc =
                                reader["MALTC"] == DBNull.Value
                                ? 0
                                : Convert.ToInt32(reader["MALTC"]),



                                NienKhoa =
                                reader["NIENKHOA"] == DBNull.Value
                                ? string.Empty
                                : reader["NIENKHOA"].ToString()!,



                                HocKy =
                                reader["HOCKY"] == DBNull.Value
                                ? 0
                                : Convert.ToInt32(reader["HOCKY"]),



                                SiSoToiDa =
                                reader["SISO_TOIDA"] == DBNull.Value
                                ? 0
                                : Convert.ToInt32(reader["SISO_TOIDA"]),



                                DayThuTrongTuan =
                                reader["DAY_THUTRONGTUAN"] == DBNull.Value
                                ? null
                                : reader["DAY_THUTRONGTUAN"].ToString(),



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



                                HuyLop =
                                reader["HUYLOP"] != DBNull.Value
                                &&
                                Convert.ToBoolean(reader["HUYLOP"]),




                                // Môn học

                                MaMh =
                                reader["MAMH"] == DBNull.Value
                                ? string.Empty
                                : reader["MAMH"].ToString()!,



                                TenMh =
                                reader["TENMH"] == DBNull.Value
                                ? string.Empty
                                : reader["TENMH"].ToString()!,



                                SoTietLt =
                                reader["SOTIET_LT"] == DBNull.Value
                                ? 0
                                : Convert.ToInt32(reader["SOTIET_LT"]),



                                SoTietTh =
                                reader["SOTIET_TH"] == DBNull.Value
                                ? 0
                                : Convert.ToInt32(reader["SOTIET_TH"]),



                                SoTinChi =
                                reader["SOTINCHI"] == DBNull.Value
                                ? 0
                                : Convert.ToInt32(reader["SOTINCHI"]),





                                // Giảng viên

                                MaGv =
                                reader["MAGV"] == DBNull.Value
                                ? string.Empty
                                : reader["MAGV"].ToString()!,



                                TenGiangVien =
                                reader["TENGIANGVIEN"] == DBNull.Value
                                ? string.Empty
                                : reader["TENGIANGVIEN"].ToString()!,



                                Email =
                                reader["EMAIL"] == DBNull.Value
                                ? null
                                : reader["EMAIL"].ToString(),



                                SoDienThoai =
                                reader["SODIENTHOAI"] == DBNull.Value
                                ? null
                                : reader["SODIENTHOAI"].ToString(),



                                HocVi =
                                reader["HOCVI"] == DBNull.Value
                                ? null
                                : reader["HOCVI"].ToString(),



                                HocHam =
                                reader["HOCHAM"] == DBNull.Value
                                ? null
                                : reader["HOCHAM"].ToString(),



                                ChuyenMon =
                                reader["CHUYENMON"] == DBNull.Value
                                ? null
                                : reader["CHUYENMON"].ToString()

                            };

                        }

                    }

                }

            }



            if (model == null)
            {
                return NotFound(new APIResponse<object>
                {
                    Message = $"Không tìm thấy lớp tín chỉ có mã {maLtc}.",
                    Data = null
                });
            }



            return Ok(new APIResponse<ChiTietLTCModel>
            {
                Message = "Lấy chi tiết lớp tín chỉ thành công.",
                Data = model
            });
        }

        [HttpPut("update/{maLtc}")]
        public async Task<IActionResult> Update(int maLtc, CapNhatLTCModel model)
        {
            if (model == null)
            {
                return BadRequest(new APIResponse<object>
                {
                    Message = "Dữ liệu cập nhật không được để trống.",
                    Data = null
                });
            }


            if (model.MaLtc <= 0)
            {
                return BadRequest(new APIResponse<object>
                {
                    Message = "Mã lớp tín chỉ không hợp lệ.",
                    Data = null
                });
            }



            try
            {

                using (SqlConnection conn = new SqlConnection(
                    _configuration.GetConnectionString("DefaultConnection")))
                {

                    using (SqlCommand cmd = new SqlCommand(
                        "SP_CAPNHAT_LTC",
                        conn))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;



                        cmd.Parameters.Add("@MALTC",
                            SqlDbType.Int)
                            .Value = model.MaLtc;



                        cmd.Parameters.Add("@NIENKHOA",
                            SqlDbType.VarChar, 20)
                            .Value = model.NienKhoa.Trim();



                        cmd.Parameters.Add("@HOCKY",
                            SqlDbType.Int)
                            .Value = model.HocKy;



                        cmd.Parameters.Add("@MAMH",
                            SqlDbType.VarChar, 20)
                            .Value = model.MaMh.Trim();



                        cmd.Parameters.Add("@MAGV",
                            SqlDbType.VarChar, 20)
                            .Value = model.MaGv.Trim();



                        cmd.Parameters.Add("@SISO_TOIDA",
                            SqlDbType.Int)
                            .Value = model.SiSoToiDa;



                        cmd.Parameters.Add("@DAY_THUTRONGTUAN",
                            SqlDbType.NVarChar, 50)
                            .Value = model.DayThuTrongTuan;



                        cmd.Parameters.Add("@THOIGIAN_BATDAU",
                            SqlDbType.DateTime)
                            .Value = model.ThoiGianBatDau;



                        cmd.Parameters.Add("@THOIGIAN_KETTHUC",
                            SqlDbType.DateTime)
                            .Value = model.ThoiGianKetThuc;



                        cmd.Parameters.Add("@HUYLOP",
                            SqlDbType.Bit)
                            .Value = model.HuyLop;



                        await conn.OpenAsync();



                        using (SqlDataReader reader =
                            await cmd.ExecuteReaderAsync())
                        {

                            if (await reader.ReadAsync())
                            {

                                return Ok(new APIResponse<object>
                                {
                                    Message = "Cập nhật lớp tín chỉ thành công.",
                                    Data = null
                                });

                            }

                        }

                    }

                }



                return BadRequest(new APIResponse<object>
                {
                    Message = "Không thể cập nhật lớp tín chỉ.",
                    Data = null
                });


            }
            catch (SqlException ex)
            {

                return BadRequest(new APIResponse<object>
                {
                    Message = ex.Message,
                    Data = null
                });

            }
            catch (Exception ex)
            {

                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new APIResponse<object>
                    {
                        Message = "Lỗi hệ thống: " + ex.Message,
                        Data = null
                    });

            }

        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(ThemLTCModel model)
        {
            if (model == null)
            {
                return BadRequest(new APIResponse<object>
                {
                    Message = "Dữ liệu không được để trống.",
                    Data = null
                });
            }



            if (string.IsNullOrWhiteSpace(model.MaMh))
            {
                return BadRequest(new APIResponse<object>
                {
                    Message = "Mã môn học không được để trống.",
                    Data = null
                });
            }



            if (string.IsNullOrWhiteSpace(model.MaGv))
            {
                return BadRequest(new APIResponse<object>
                {
                    Message = "Mã giảng viên không được để trống.",
                    Data = null
                });
            }



            int maLtc = 0;



            try
            {

                using (SqlConnection conn = new SqlConnection(
                    _configuration.GetConnectionString("DefaultConnection")))
                {

                    using (SqlCommand cmd = new SqlCommand(
                        "SP_THEM_LTC",
                        conn))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;



                        cmd.Parameters.Add("@NIENKHOA",
                            SqlDbType.VarChar, 20)
                            .Value = model.NienKhoa.Trim();



                        cmd.Parameters.Add("@HOCKY",
                            SqlDbType.Int)
                            .Value = model.HocKy;



                        cmd.Parameters.Add("@MAMH",
                            SqlDbType.VarChar, 20)
                            .Value = model.MaMh.Trim();



                        cmd.Parameters.Add("@MAGV",
                            SqlDbType.VarChar, 20)
                            .Value = model.MaGv.Trim();



                        cmd.Parameters.Add("@SISO_TOIDA",
                            SqlDbType.Int)
                            .Value = model.SiSoToiDa;



                        cmd.Parameters.Add("@DAY_THUTRONGTUAN",
                            SqlDbType.NVarChar, 50)
                            .Value = model.DayThuTrongTuan;



                        cmd.Parameters.Add("@THOIGIAN_BATDAU",
                            SqlDbType.DateTime)
                            .Value = model.ThoiGianBatDau;



                        cmd.Parameters.Add("@THOIGIAN_KETTHUC",
                            SqlDbType.DateTime)
                            .Value = model.ThoiGianKetThuc;



                        cmd.Parameters.Add("@HUYLOP",
                            SqlDbType.Bit)
                            .Value = model.HuyLop;



                        await conn.OpenAsync();



                        using (SqlDataReader reader =
                            await cmd.ExecuteReaderAsync())
                        {

                            if (await reader.ReadAsync())
                            {

                                maLtc = reader["MALTC"] == DBNull.Value
                                    ? 0
                                    : Convert.ToInt32(reader["MALTC"]);

                            }

                        }

                    }

                }



                return Ok(new APIResponse<object>
                {
                    Message = "Thêm lớp tín chỉ thành công.",
                    Data = null
                });


            }
            catch (SqlException ex)
            {

                return BadRequest(new APIResponse<object>
                {
                    Message = ex.Message,
                    Data = null
                });

            }
            catch (Exception ex)
            {

                return StatusCode(500,
                    new APIResponse<object>
                    {
                        Message = "Lỗi hệ thống: " + ex.Message,
                        Data = null
                    });

            }
        }
    }
}
