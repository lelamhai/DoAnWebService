using DoAnWebService.DTO.Classroom;
using DoAnWebService.DTO.Student;
using DoAnWebService.DTO.Teacher;
using DoAnWebService.Utils;
using DoAnWebService.Utlis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace DoAnWebService.Controllers
{
    [Route("api/v1/private/[controller]")]
    [ApiController]
    //[Authorize(Roles = "NV")]
    public class TeacherController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public TeacherController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("get-faculty")]
        public async Task<IActionResult> GetFaculty()
        {
            List<FacultyModel> list = new();
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand cmd = new SqlCommand("SP_LAYKHOA", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    await conn.OpenAsync();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            list.Add(new FacultyModel
                            {
                                Makhoa = reader["MAKHOA"].ToString(),
                                Tenkhoa = reader["TENKHOA"].ToString()
                            });
                        }
                    }
                }
            }
            return Ok(new APIResponse<List<FacultyModel>>
            {
                Message = "Lấy danh sách khoa thành công.",
                Data = list
            });
        }

        [HttpGet("get-status-student")]
        public async Task<IActionResult> GetStatusStudent()
        {
            List<StatusTableModel> list = new();
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand cmd = new SqlCommand("SP_LAYTRANGTHAI_SV", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    await conn.OpenAsync();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            list.Add(new StatusTableModel
                            {
                                ID = (int)reader["ID"],
                                Name = reader["TENTRANGTHAI"].ToString()
                            });
                        }
                    }
                }
            }
            return Ok(new APIResponse<List<StatusTableModel>>
            {
                Message = "Lấy danh sách trạng thái lớp thành công.",
                Data = list
            });
        }

        [HttpGet("get-teachers")]
        public async Task<IActionResult> GetTeachers(int page)
        {
            List<TeacherModel> list = new();
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand cmd = new SqlCommand("SP_LAYDS_GV", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    await conn.OpenAsync();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            list.Add(new TeacherModel
                            {
                                Makhoa = reader["MAKHOA"] == DBNull.Value
                                ? null
                                : reader["MAKHOA"].ToString()?.Trim(),

                                Tenkhoa = reader["TENKHOA"] == DBNull.Value
                                ? null
                                : reader["TENKHOA"].ToString()?.Trim(),

                                Magv = reader["MAGV"] == DBNull.Value
                                ? null
                                : reader["MAGV"].ToString()?.Trim(),

                                Ho = reader["HO"] == DBNull.Value
                                ? null
                                : reader["HO"].ToString()?.Trim(),

                                Ten = reader["TEN"] == DBNull.Value
                                ? null
                                : reader["TEN"].ToString()?.Trim(),

                                Phai = reader["PHAI"] == DBNull.Value
                                ? null
                                : Convert.ToBoolean(reader["PHAI"])
                                    ? "Nam"
                                    : "Nữ",

                                Diachi = reader["DIACHI"] == DBNull.Value
                                ? null
                                : reader["DIACHI"].ToString()?.Trim(),

                                Sodienthoai =
                                reader["SODIENTHOAI"] == DBNull.Value
                                    ? null
                                    : reader["SODIENTHOAI"]
                                        .ToString()
                                        ?.Trim(),

                                Ngaysinh = reader["NGAYSINH"] == DBNull.Value
                                ? null
                                : Convert.ToDateTime(reader["NGAYSINH"])
                                    .ToString("dd/MM/yyyy"),

                                Email = reader["EMAIL"] == DBNull.Value
                                ? null
                                : reader["EMAIL"].ToString()?.Trim(),

                                Hocvi = reader["HOCVI"] == DBNull.Value
                                ? null
                                : reader["HOCVI"].ToString()?.Trim(),

                                Hocham = reader["HOCHAM"] == DBNull.Value
                                ? null
                                : reader["HOCHAM"].ToString()?.Trim(),

                                Chuyenmon =
                                reader["CHUYENMON"] == DBNull.Value
                                    ? null
                                    : reader["CHUYENMON"]
                                        .ToString()
                                        ?.Trim(),

                                Id = reader["ID"] == DBNull.Value
                                ? 0
                                : Convert.ToInt32(reader["ID"]),

                                Tentrangthai =
                                reader["TENTRANGTHAI"] == DBNull.Value
                                    ? null
                                    : reader["TENTRANGTHAI"]
                                        .ToString()
                                        ?.Trim()
                            });

                        }
                    }
                }
            }

            var result = PaginationHelper.CreatePagedResult(list, page, -1);
            return Ok(new APIResponse<PagedResult<TeacherModel>>
            {
                Message = "Lấy danh sách giảng viên thành công.",
                Data = result
            });
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchTeachers(string? keyword)
        {
            List<TeacherModel> list = new();
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand cmd = new SqlCommand("SP_TIMKIEM_GV", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@KEYWORD", keyword ?? string.Empty);
                    await conn.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            list.Add(new TeacherModel
                            {
                                Makhoa = reader["MAKHOA"] == DBNull.Value
                                ? null
                                : reader["MAKHOA"].ToString()?.Trim(),

                                Tenkhoa = reader["TENKHOA"] == DBNull.Value
                                ? null
                                : reader["TENKHOA"].ToString()?.Trim(),

                                Magv = reader["MAGV"] == DBNull.Value
                                ? null
                                : reader["MAGV"].ToString()?.Trim(),

                                Ho = reader["HO"] == DBNull.Value
                                ? null
                                : reader["HO"].ToString()?.Trim(),

                                Ten = reader["TEN"] == DBNull.Value
                                ? null
                                : reader["TEN"].ToString()?.Trim(),

                                Phai = reader["PHAI"] == DBNull.Value
                                ? null
                                : Convert.ToBoolean(reader["PHAI"])
                                    ? "Nam"
                                    : "Nữ",

                                Diachi = reader["DIACHI"] == DBNull.Value
                                ? null
                                : reader["DIACHI"].ToString()?.Trim(),

                                Sodienthoai =
                                reader["SODIENTHOAI"] == DBNull.Value
                                    ? null
                                    : reader["SODIENTHOAI"]
                                        .ToString()
                                        ?.Trim(),

                                Ngaysinh = reader["NGAYSINH"] == DBNull.Value
                                ? null
                                : Convert.ToDateTime(reader["NGAYSINH"])
                                    .ToString("dd/MM/yyyy"),

                                Email = reader["EMAIL"] == DBNull.Value
                                ? null
                                : reader["EMAIL"].ToString()?.Trim(),

                                Hocvi = reader["HOCVI"] == DBNull.Value
                                ? null
                                : reader["HOCVI"].ToString()?.Trim(),

                                Hocham = reader["HOCHAM"] == DBNull.Value
                                ? null
                                : reader["HOCHAM"].ToString()?.Trim(),

                                Chuyenmon =
                                reader["CHUYENMON"] == DBNull.Value
                                    ? null
                                    : reader["CHUYENMON"]
                                        .ToString()
                                        ?.Trim(),

                                Id = reader["ID"] == DBNull.Value
                                ? 0
                                : Convert.ToInt32(reader["ID"]),

                                Tentrangthai =
                                reader["TENTRANGTHAI"] == DBNull.Value
                                    ? null
                                    : reader["TENTRANGTHAI"]
                                        .ToString()
                                        ?.Trim()
                            });
                        }
                    }
                }
            }
            var result = PaginationHelper.CreatePagedResult(list, 1, -1);
            return Ok(new APIResponse<PagedResult<TeacherModel>>
            {
                Message = $"Tìm kiếm lớp với từ khóa '{keyword}' thành công.",
                Data = result
            });
        }

        [HttpDelete("delete/{magv}")]
        public async Task<IActionResult> Delete(string magv)
        {
            try
            {
                using var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
                using var cmd = new SqlCommand("SP_XOA_GV", conn);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@MAGV", SqlDbType.NVarChar, 100).Value = magv;

                var messageParam = new SqlParameter("@MESSAGE", SqlDbType.NVarChar, 200)
                {
                    Direction = ParameterDirection.Output
                };

                cmd.Parameters.Add(messageParam);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                string message = messageParam.Value?.ToString();

                if (message == "1")
                {
                    return Ok(new
                    {
                        message = "Xóa giảng viên thành công"
                    });
                }

                return BadRequest(new
                {
                    message = "Không tìm thấy  giảng viên"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Sinh viên không xóa được"
                });
            }
        }

        [HttpGet("detail/{magv}")]
        public async Task<IActionResult> GetGVByMaGV(string magv)
        {
            try
            {
                using var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
                using var cmd = new SqlCommand("SP_LAYMOT_GV", conn);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@MAGV", SqlDbType.NVarChar, 100).Value = magv;

                var messageParam = new SqlParameter("@MESSAGE", SqlDbType.NVarChar, 200)
                {
                    Direction = ParameterDirection.Output
                };

                cmd.Parameters.Add(messageParam);

                await conn.OpenAsync();
                List<TeacherModel> list = new();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        list.Add(new TeacherModel
                        {
                            Makhoa = reader["MAKHOA"] == DBNull.Value
                                ? null
                                : reader["MAKHOA"].ToString()?.Trim(),

                            Tenkhoa = reader["TENKHOA"] == DBNull.Value
                                ? null
                                : reader["TENKHOA"].ToString()?.Trim(),

                            Magv = reader["MAGV"] == DBNull.Value
                                ? null
                                : reader["MAGV"].ToString()?.Trim(),

                            Ho = reader["HO"] == DBNull.Value
                                ? null
                                : reader["HO"].ToString()?.Trim(),

                            Ten = reader["TEN"] == DBNull.Value
                                ? null
                                : reader["TEN"].ToString()?.Trim(),

                            Phai = reader["PHAI"] == DBNull.Value
                                ? null
                                : Convert.ToBoolean(reader["PHAI"])
                                    ? "Nam"
                                    : "Nữ",

                            Diachi = reader["DIACHI"] == DBNull.Value
                                ? null
                                : reader["DIACHI"].ToString()?.Trim(),

                            Sodienthoai =
                                reader["SODIENTHOAI"] == DBNull.Value
                                    ? null
                                    : reader["SODIENTHOAI"]
                                        .ToString()
                                        ?.Trim(),

                            Ngaysinh = reader["NGAYSINH"] == DBNull.Value
                                ? null
                                : Convert.ToDateTime(reader["NGAYSINH"])
                                    .ToString("dd/MM/yyyy"),

                            Email = reader["EMAIL"] == DBNull.Value
                                ? null
                                : reader["EMAIL"].ToString()?.Trim(),

                            Hocvi = reader["HOCVI"] == DBNull.Value
                                ? null
                                : reader["HOCVI"].ToString()?.Trim(),

                            Hocham = reader["HOCHAM"] == DBNull.Value
                                ? null
                                : reader["HOCHAM"].ToString()?.Trim(),

                            Chuyenmon =
                                reader["CHUYENMON"] == DBNull.Value
                                    ? null
                                    : reader["CHUYENMON"]
                                        .ToString()
                                        ?.Trim(),

                            Id = reader["ID"] == DBNull.Value
                                ? 0
                                : Convert.ToInt32(reader["ID"]),

                            Tentrangthai =
                                reader["TENTRANGTHAI"] == DBNull.Value
                                    ? null
                                    : reader["TENTRANGTHAI"]
                                        .ToString()
                                        ?.Trim()
                        });
                    }
                }

                string message = messageParam.Value?.ToString();

                if (message == "0" || list == null)
                {
                    return NotFound(new
                    {
                        message = "Không tìm thấy sinh viên"
                    });
                }

                return Ok(new APIResponse<List<TeacherModel>>
                {
                    Message = "Lấy chi tiết một giảng viên thành công.",
                    Data = list
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = "Lỗi hệ thống"
                });
            }
        }

        [HttpPut("update/{magv}")]
        public async Task<IActionResult> Update(string magv, UpdateTeacherModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(magv))
                {
                    return BadRequest(new
                    {
                        message = "Mã giảng viên không được để trống"
                    });
                }

                if (!DateTime.TryParseExact(
                    model.Ngaysinh,
                    new[] { "dd/MM/yyyy", "yyyy-MM-dd" },
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime ngaySinh))
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

                using var cmd = new SqlCommand("SP_CAPNHAT_GV", conn);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@MAGV", SqlDbType.NChar, 10)
                    .Value = magv.Trim();

                cmd.Parameters.Add("@MAKHOA", SqlDbType.NChar, 10)
                    .Value = model.Makhoa.Trim();

                cmd.Parameters.Add("@HO", SqlDbType.NVarChar, 50)
                    .Value = model.Ho.Trim();

                cmd.Parameters.Add("@TEN", SqlDbType.NVarChar, 50)
                    .Value = model.Ten.Trim();

                cmd.Parameters.Add("@PHAI", SqlDbType.Bit)
                    .Value = model.Phai;

                cmd.Parameters.Add("@DIACHI", SqlDbType.NVarChar, 100)
                    .Value = string.IsNullOrWhiteSpace(model.Diachi)
                        ? DBNull.Value
                        : model.Diachi.Trim();

                cmd.Parameters.Add("@SODIENTHOAI", SqlDbType.Char, 20)
                    .Value = string.IsNullOrWhiteSpace(model.Sodienthoai)
                        ? DBNull.Value
                        : model.Sodienthoai.Trim();

                cmd.Parameters.Add("@NGAYSINH", SqlDbType.Date)
                    .Value = ngaySinh;

                cmd.Parameters.Add("@EMAIL", SqlDbType.VarChar, 50)
                    .Value = string.IsNullOrWhiteSpace(model.Email)
                        ? DBNull.Value
                        : model.Email.Trim();

                cmd.Parameters.Add("@HOCVI", SqlDbType.NVarChar, 20)
                    .Value = string.IsNullOrWhiteSpace(model.Hocvi)
                        ? DBNull.Value
                        : model.Hocvi.Trim();

                cmd.Parameters.Add("@HOCHAM", SqlDbType.NVarChar, 20)
                    .Value = string.IsNullOrWhiteSpace(model.Hocham)
                        ? DBNull.Value
                        : model.Hocham.Trim();

                cmd.Parameters.Add("@CHUYENMON", SqlDbType.NVarChar, 50)
                    .Value = string.IsNullOrWhiteSpace(model.Chuyenmon)
                        ? DBNull.Value
                        : model.Chuyenmon.Trim();

                cmd.Parameters.Add("@TRANGTHAI", SqlDbType.Int)
                    .Value = model.Trangthai;

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
                    Message = "Cập nhật giảng viên thành công",
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

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateTeacherModels model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.Magv))
                {
                    return BadRequest(new
                    {
                        message = "Mã giảng viên không được để trống"
                    });
                }

                if (!DateTime.TryParseExact(
                    model.Ngaysinh,
                    new[] { "dd/MM/yyyy", "yyyy-MM-dd" },
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime ngaySinh))
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

                using var cmd = new SqlCommand("SP_THEM_GV", conn);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@MAGV", SqlDbType.NChar, 10)
                    .Value = model.Magv.Trim();

                cmd.Parameters.Add("@MAKHOA", SqlDbType.NChar, 10)
                    .Value = model.Makhoa.Trim();

                cmd.Parameters.Add("@HO", SqlDbType.NVarChar, 50)
                    .Value = model.Ho.Trim();

                cmd.Parameters.Add("@TEN", SqlDbType.NVarChar, 50)
                    .Value = model.Ten.Trim();

                cmd.Parameters.Add("@PHAI", SqlDbType.Bit)
                    .Value = model.Phai;

                cmd.Parameters.Add("@DIACHI", SqlDbType.NVarChar, 100)
                    .Value = string.IsNullOrWhiteSpace(model.Diachi)
                        ? DBNull.Value
                        : model.Diachi.Trim();

                cmd.Parameters.Add("@SODIENTHOAI", SqlDbType.Char, 20)
                    .Value = string.IsNullOrWhiteSpace(model.Sodienthoai)
                        ? DBNull.Value
                        : model.Sodienthoai.Trim();

                cmd.Parameters.Add("@NGAYSINH", SqlDbType.Date)
                    .Value = ngaySinh;

                cmd.Parameters.Add("@EMAIL", SqlDbType.VarChar, 50)
                    .Value = string.IsNullOrWhiteSpace(model.Email)
                        ? DBNull.Value
                        : model.Email.Trim();

                cmd.Parameters.Add("@HOCVI", SqlDbType.NVarChar, 20)
                    .Value = string.IsNullOrWhiteSpace(model.Hocvi)
                        ? DBNull.Value
                        : model.Hocvi.Trim();

                cmd.Parameters.Add("@HOCHAM", SqlDbType.NVarChar, 20)
                    .Value = string.IsNullOrWhiteSpace(model.Hocham)
                        ? DBNull.Value
                        : model.Hocham.Trim();

                cmd.Parameters.Add("@CHUYENMON", SqlDbType.NVarChar, 50)
                    .Value = string.IsNullOrWhiteSpace(model.Chuyenmon)
                        ? DBNull.Value
                        : model.Chuyenmon.Trim();

                cmd.Parameters.Add("@TRANGTHAI", SqlDbType.Int)
                    .Value = model.Trangthai;

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
                    Message = "Cập nhật giảng viên thành công",
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
