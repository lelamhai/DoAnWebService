using DoAnWebService.DTO.Classroom;
using DoAnWebService.DTO.Student;
using DoAnWebService.Utils;
using DoAnWebService.Utlis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace DoAnWebService.Controllers
{
    [Route("api/v1/private/[controller]")]
    [ApiController]
    //[Authorize]
    public class StudentController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public StudentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("get-classrooms")]
        public async Task<IActionResult> GetClassrooms()
        {
            List<ClassroomModelStudent> list = new();
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand cmd = new SqlCommand("SP_LAYLOP_SV", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    await conn.OpenAsync();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            list.Add(new ClassroomModelStudent
                            {
                                MALOP = reader["MALOP"].ToString(),
                                TENLOP = reader["TENLOP"].ToString()
                            });
                        }
                    }
                }
            }


            return Ok(new APIResponse<List<ClassroomModelStudent>>
            {
                Message = "Lấy danh sách trạng thái lớp thành công.",
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

        [HttpGet("get-students")]
        public async Task<IActionResult> GetStudents(int page)
        {
            List<StudentModel> list = new();
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand cmd = new SqlCommand("SP_LAYDS_SV", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    await conn.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            list.Add(new StudentModel
                            {
                                Masv = reader["MASV"].ToString(),
                                Ho = reader["HO"].ToString(),
                                Ten = reader["TEN"].ToString(),
                                Phai = (bool)reader["PHAI"] ? "Nam" : "Nữ",
                                Diachi = reader["DIACHI"].ToString(),
                                Sodienthoai = reader["SODIENTHOAI"].ToString(),
                                Ngaysinh = Convert.ToDateTime(reader["NGAYSINH"]).ToString("dd/MM/yyyy"),
                                Email = reader["EMAIL"].ToString(),
                                Tenlop = reader["MALOP"].ToString(),
                                Trangthai = reader["TENTRANGTHAI"].ToString()
                            });
                        }
                    }
                }
            }
            var result = PaginationHelper.CreatePagedResult(list, page, -1);
            return Ok(new APIResponse<PagedResult<StudentModel>>
            {
                Message = "Lấy danh sách lớp thành công.",
                Data = result
            });
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchClassrooms(string? keyword)
        {
            List<StudentModel> list = new();
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand cmd = new SqlCommand("SP_TIMKIEM_SV", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@KEYWORD", keyword ?? string.Empty);
                    await conn.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            list.Add(new StudentModel
                            {
                                Masv = reader["MASV"].ToString(),
                                Ho = reader["HO"].ToString(),
                                Ten = reader["TEN"].ToString(),
                                Phai = (bool)reader["PHAI"] ? "Nam" : "Nữ",
                                Diachi = reader["DIACHI"].ToString(),
                                Sodienthoai = reader["SODIENTHOAI"].ToString(),
                                Ngaysinh = Convert.ToDateTime(reader["NGAYSINH"]).ToString("dd/MM/yyyy"),
                                Email = reader["EMAIL"].ToString(),
                                Tenlop = reader["MALOP"].ToString(),
                                Trangthai = reader["TENTRANGTHAI"].ToString()
                            });
                        }
                    }
                }
            }
            var result = PaginationHelper.CreatePagedResult(list, 1, -1);
            return Ok(new APIResponse<PagedResult<StudentModel>>
            {
                Message = $"Tìm kiếm lớp với từ khóa '{keyword}' thành công.",
                Data = result
            });
        }


        [HttpDelete("delete/{masv}")]
        public async Task<IActionResult> Delete(string masv)
        {
            try
            {
                using var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
                using var cmd = new SqlCommand("SP_XOA_SV", conn);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@MASV", SqlDbType.NVarChar, 100).Value = masv;

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
                        message = "Xóa sinh viên thành công"
                    });
                }

                return BadRequest(new
                {
                    message = "Không tìm thấy sinh viên"
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

        [HttpGet("detail/{masv}")]
        public async Task<IActionResult> GetSVByMaSV(string masv)
        {
            try
            {
                using var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
                using var cmd = new SqlCommand("SP_LAYMOT_SV", conn);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@MASV", SqlDbType.NVarChar, 100).Value = masv;

                var messageParam = new SqlParameter("@MESSAGE", SqlDbType.NVarChar, 200)
                {
                    Direction = ParameterDirection.Output
                };

                cmd.Parameters.Add(messageParam);

                await conn.OpenAsync();
                List<StudentModel> list = new();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        list.Add(new StudentModel
                        {
                            Masv = reader["MASV"].ToString(),
                            Ho = reader["HO"].ToString(),
                            Ten = reader["TEN"].ToString(),
                            Phai = (bool)reader["PHAI"] ? "Nam" : "Nữ",
                            Diachi = reader["DIACHI"].ToString(),
                            Sodienthoai = reader["SODIENTHOAI"].ToString(),
                            Ngaysinh = Convert.ToDateTime(reader["NGAYSINH"]).ToString("dd/MM/yyyy"),
                            Email = reader["EMAIL"].ToString(),
                            Malop = reader["MALOP"].ToString(),
                            Tenlop = reader["TENLOP"].ToString(),
                            Id = (int)reader["ID"],
                            Trangthai = reader["TENTRANGTHAI"].ToString()
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

                return Ok(new APIResponse<List<StudentModel>>
                {
                    Message = "Lấy chi tiết một sinh viên thành công.",
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

        [HttpPut("update/{masv}")]
        public async Task<IActionResult> Update(string masv, UpdateStudent model)
        {
            try
            {
                if (!DateTime.TryParseExact(
                model.Ngaysinh,
                new[] { "dd/MM/yyyy", "yyyy-MM-dd" },
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime ngaySinh))
                {
                    return BadRequest(new
                    {
                        message = "Ngày sinh không đúng định dạng. Vui lòng nhập dd/MM/yyyy."
                    });
                }

                using var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
                using var cmd = new SqlCommand("SP_CAPNHAT_SV", conn);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@MASV", SqlDbType.NChar, 10).Value = masv;
                cmd.Parameters.Add("@HO", SqlDbType.NVarChar, 50).Value = model.Ho;
                cmd.Parameters.Add("@TEN", SqlDbType.NVarChar, 50).Value = model.Ten;
                cmd.Parameters.Add("@PHAI", SqlDbType.Bit).Value = model.Phai;
                cmd.Parameters.Add("@DIACHI", SqlDbType.NVarChar, 100).Value = model.Diachi;
                cmd.Parameters.Add("@SODIENTHOAI", SqlDbType.Char, 20).Value = model.Sodienthoai;
                cmd.Parameters.Add("@NGAYSINH", SqlDbType.Date).Value = ngaySinh;
                cmd.Parameters.Add("@EMAIL", SqlDbType.VarChar, 50).Value = model.Email;
                cmd.Parameters.Add("@MALOP", SqlDbType.NChar, 10).Value = model.Malop;
                cmd.Parameters.Add("@TRANGTHAI", SqlDbType.Int).Value = model.Trangthai;

                var messageParam = new SqlParameter("@MESSAGE", SqlDbType.NVarChar, 200)
                {
                    Direction = ParameterDirection.Output
                };

                cmd.Parameters.Add(messageParam);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                string message = messageParam.Value?.ToString() ?? "0";

                if (message == "0")
                {
                    return NotFound(new APIResponse<List<StudentModel>>
                    {
                        Message = "Không tìm thấy sinh viên",
                        Data = null
                    });
                }

                return Ok(new APIResponse<List<StudentModel>>
                {
                    Message = "Cập nhật sinh viên thành công",
                    Data = null
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

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateStudent model)
        {
            try
            {
                if (!DateTime.TryParseExact(
               model.Ngaysinh,
               new[] { "dd/MM/yyyy", "yyyy-MM-dd" },
               CultureInfo.InvariantCulture,
               DateTimeStyles.None,
               out DateTime ngaySinh))
                {
                    return BadRequest(new
                    {
                        message = "Ngày sinh không đúng định dạng. Vui lòng nhập dd/MM/yyyy."
                    });
                }

                using var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
                using var cmd = new SqlCommand("SP_THEM_SV", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@MASV", SqlDbType.NChar, 10).Value = model.Masv;
                cmd.Parameters.Add("@HO", SqlDbType.NVarChar, 50).Value = model.Ho;
                cmd.Parameters.Add("@TEN", SqlDbType.NVarChar, 50).Value = model.Ten;
                cmd.Parameters.Add("@PHAI", SqlDbType.Bit).Value = model.Phai;
                cmd.Parameters.Add("@DIACHI", SqlDbType.NVarChar, 100).Value = model.Diachi;
                cmd.Parameters.Add("@SODIENTHOAI", SqlDbType.Char, 20).Value = model.Sodienthoai;
                cmd.Parameters.Add("@NGAYSINH", SqlDbType.Date).Value = ngaySinh;
                cmd.Parameters.Add("@EMAIL", SqlDbType.VarChar, 50).Value = model.Email;
                cmd.Parameters.Add("@MALOP", SqlDbType.NChar, 10).Value = model.Malop;
                cmd.Parameters.Add("@TRANGTHAI", SqlDbType.Int).Value = model.Trangthai;
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
                    return BadRequest(new APIResponse<List<StudentModel>>
                    {
                        Message = "Đã có mã sinh viên tồn tại",
                        Data = null
                    });
                }
                return Ok(new APIResponse<List<StudentModel>>
                {
                    Message = "Thêm sinh viên thành công",
                    Data = null
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

    }
}
