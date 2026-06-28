using DoAnWebService.Data;
using DoAnWebService.DTO.Classroom;
using DoAnWebService.Models;
using DoAnWebService.Utils;
using DoAnWebService.Utlis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace DoAnWebService.Controllers
{
    [Route("api/v1/private/[controller]")]
    [ApiController]
    //[Authorize]
    public class ClassroomController : ControllerBase
    {
        
        private readonly IConfiguration _configuration;

        public ClassroomController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("get-classrooms")]
        public async Task<IActionResult> GetClassrooms(int page = 1)
        {
            List<ClassroomModel> list = new();

            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand cmd = new SqlCommand("SP_LAYLOP", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    await conn.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            list.Add(new ClassroomModel
                            {
                                Malop = reader["MALOP"].ToString(),
                                Tenlop = reader["TENLOP"].ToString(),
                                Khoahoc = reader["KHOAHOC"].ToString(),
                                Tenkhoa = reader["TENKHOA"].ToString(),
                                Tennv = reader["HOTENNV"].ToString(),
                                Ngaymolop = Convert.ToDateTime(reader["NGAYMOLOP"]).ToString("dd/MM/yyyy"),
                                Trangthai = reader["TENTRANGTHAI"].ToString()
                            });
                        }
                    }
                }
            }
            var result = PaginationHelper.CreatePagedResult(list, page, -1);
            return Ok(new APIResponse<PagedResult<ClassroomModel>>
            {
                Message = "Lấy danh sách lớp thành công.",
                Data = result
            });
        }

        [HttpGet("get-status-classrooms")]
        public async Task<IActionResult> GetStatusClassrooms()
        {
            List<StatusTableModel> list = new();
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand cmd = new SqlCommand("SP_LAYTRANGTHAILOP", conn))
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

        [HttpGet("search")]
        public async Task<IActionResult> SearchClassrooms(string? keyword)
        {   
            List<ClassroomModel> list = new();
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand cmd = new SqlCommand("SP_TIMKIEM_LOP", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@KEYWORD", keyword ?? string.Empty);
                    await conn.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            list.Add(new ClassroomModel
                            {
                                Malop = reader["MALOP"].ToString(),
                                Tenlop = reader["TENLOP"].ToString(),
                                Khoahoc = reader["KHOAHOC"].ToString(),
                                Tenkhoa = reader["TENKHOA"].ToString(),
                                Tennv = reader["HOTENNV"].ToString(),
                                Ngaymolop = Convert.ToDateTime(reader["NGAYMOLOP"]).ToString("dd/MM/yyyy"),
                                Trangthai = reader["TENTRANGTHAI"].ToString()
                            });
                        }
                    }
                }
            }
            var result = PaginationHelper.CreatePagedResult(list, 1, -1);
            return Ok(new APIResponse<PagedResult<ClassroomModel>>
            {
                Message = $"Tìm kiếm lớp với từ khóa '{keyword}' thành công.",
                Data = result
            });
        }

        [HttpDelete("delete/{malop}")]
        public async Task<IActionResult> Delete(string malop)
        {
            using SqlConnection conn = new SqlConnection( _configuration.GetConnectionString("DefaultConnection"));

            using SqlCommand cmd = new SqlCommand("SP_XOA_LOP", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@MALOP", malop);

            await conn.OpenAsync();

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                bool canDelete = reader.GetBoolean(reader.GetOrdinal("CanDelete"));
                string message = reader.GetString(reader.GetOrdinal("Message"));

                if(canDelete)
                {
                    return Ok(new
                    {
                        message = message
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        message = message
                    });
                }
            }

            return BadRequest(new APIResponse<string>
            {
                Message = "Không thể thực hiện xóa lớp."
            });
        }

        [HttpGet("detail/{malop}")]
        public async Task<ActionResult<Lop>> GetLopByMaLop(string malop)
        {
            List<DetailModel> list = new();
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand cmd = new SqlCommand("SP_LAYMOT_LOP", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MALOP", malop ?? string.Empty);
                    await conn.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            list.Add(new DetailModel
                            {
                                Malop = reader["MALOP"].ToString(),
                                Tenlop = reader["TENLOP"].ToString(),
                                Khoahoc = reader["KHOAHOC"].ToString(),
                                Makhoa = reader["MAKHOA"].ToString(),
                                Tennv = reader["HOTENNV"].ToString(),
                                Ngaymolop = Convert.ToDateTime(reader["NGAYMOLOP"]).ToString("dd/MM/yyyy"),
                                Trangthai = (int)reader["TRANGTHAI"]
                            });
                        }
                    }
                }
            }
            
            return Ok(new APIResponse<List<DetailModel>>
            {
                Message = "Lấy chi tiết một lớp thành công.",
                Data = list
            });

        }


        [HttpPut("update/{malop}")]
        public async Task<IActionResult> Update(string malop, UpdateModel model)
        {
            if (string.IsNullOrWhiteSpace(malop))
            {
                return BadRequest(new
                {
                    message = "Mã lớp không hợp lệ."
                });
            }

            if (model == null)
            {
                return BadRequest(new
                {
                    message = "Dữ liệu cập nhật không hợp lệ."
                });
            }

            if (!DateTime.TryParseExact(
                model.NgayMoLop,
                new[] { "dd/MM/yyyy", "yyyy-MM-dd" },
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime ngayMoLop))
            {
                return BadRequest(new
                {
                    message = "Ngày mở lớp không đúng định dạng. Vui lòng nhập dd/MM/yyyy."
                });
            }

            try
            {
                using var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
                using var cmd = new SqlCommand("SP_CAPNHAT_LOP", conn);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@MALOP", SqlDbType.NVarChar, 20).Value = malop;
                cmd.Parameters.Add("@TENLOP", SqlDbType.NVarChar, 100).Value = model.TenLop;
                cmd.Parameters.Add("@KHOAHOC", SqlDbType.NVarChar, 20).Value = model.KhoaHoc;
                cmd.Parameters.Add("@MAKHOA", SqlDbType.NVarChar, 20).Value = model.MaKhoa;
                cmd.Parameters.Add("@MANV", SqlDbType.NVarChar, 20).Value = "NV00000001";
                cmd.Parameters.Add("@NGAYMOLOP", SqlDbType.Date).Value = ngayMoLop;
                cmd.Parameters.Add("@TRANGTHAI", SqlDbType.Int).Value = model.TrangThai;

                await conn.OpenAsync();

                var result = await cmd.ExecuteNonQueryAsync();

                return Ok(new
                {
                    message = result.ToString() ?? "Cập nhật lớp thành công."
                });
            }
            catch (SqlException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Có lỗi xảy ra khi cập nhật lớp.",
                    error = ex.Message
                });
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateClassroom(UpdateModel model)
        {
            if (string.IsNullOrWhiteSpace(model.MaLop))
            {
                return BadRequest(new
                {
                    message = "Mã lớp không hợp lệ."
                });
            }

            if (model == null)
            {
                return BadRequest(new
                {
                    message = "Dữ liệu cập nhật không hợp lệ."
                });
            }

            if (!DateTime.TryParseExact(
                model.NgayMoLop,
                new[] { "dd/MM/yyyy", "yyyy-MM-dd" },
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime ngayMoLop))
            {
                return BadRequest(new
                {
                    message = "Ngày mở lớp không đúng định dạng. Vui lòng nhập dd/MM/yyyy."
                });
            }

            try
            {
                using var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
                using var cmd = new SqlCommand("SP_TAO_LOP", conn);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@MALOP", SqlDbType.NVarChar, 20).Value = model.MaLop;
                cmd.Parameters.Add("@TENLOP", SqlDbType.NVarChar, 100).Value = model.TenLop;
                cmd.Parameters.Add("@KHOAHOC", SqlDbType.NVarChar, 20).Value = model.KhoaHoc;
                cmd.Parameters.Add("@MAKHOA", SqlDbType.NVarChar, 20).Value = model.MaKhoa;
                cmd.Parameters.Add("@MANV", SqlDbType.NVarChar, 20).Value = "NV00000001";
                cmd.Parameters.Add("@NGAYMOLOP", SqlDbType.Date).Value = ngayMoLop;
                cmd.Parameters.Add("@TRANGTHAI", SqlDbType.Int).Value = model.TrangThai;

                var messageParam = new SqlParameter("@MESSAGE", SqlDbType.NVarChar, 200)
                {
                    Direction = ParameterDirection.Output
                };

                cmd.Parameters.Add(messageParam);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                var result = messageParam.Value?.ToString();
                if(result == "0")
                {
                    return Ok(new APIResponse<string>
                    {
                        Message = "Tạo lớp thành công."
                    });
                }    

                return BadRequest(new APIResponse<string>
                {
                    Message = "Tạo lớp thất bại. Mã lớp đã tồn tại."
                });
            }
            catch (SqlException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Có lỗi xảy ra khi cập nhật lớp.",
                    error = ex.Message
                });
            }
        }
    }
}
