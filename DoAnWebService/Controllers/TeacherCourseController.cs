using DoAnWebService.DTO.Student;
using DoAnWebService.DTO.Teacher;
using DoAnWebService.Utils;
using DoAnWebService.Utlis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DoAnWebService.Controllers
{
    [Route("api/v1/private/[controller]")]
    [ApiController]
    //[Authorize(Roles = "GV")]
    public class TeacherCourseController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public TeacherCourseController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("get-list")]
        public async Task<IActionResult> GetTeacherSubject(string maGv)
        {
            if (string.IsNullOrWhiteSpace(maGv))
            {
                return BadRequest(new APIResponse<object>
                {
                    Message = "Mã giảng viên không được để trống.",
                    Data = null
                });
            }



            List<LTC_GVModel> list = new();



            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection")))
            {

                using (SqlCommand cmd = new SqlCommand(
                    "SP_LAYDS_LTC_GV",
                    conn))
                {

                    cmd.CommandType = CommandType.StoredProcedure;



                    cmd.Parameters.Add("@MAGV",
                        SqlDbType.VarChar, 20)
                        .Value = maGv.Trim();



                    await conn.OpenAsync();



                    using (SqlDataReader reader =
                        await cmd.ExecuteReaderAsync())
                    {

                        while (await reader.ReadAsync())
                        {

                            list.Add(new LTC_GVModel
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



                                MaMh =
                                reader["MAMH"] == DBNull.Value
                                ? string.Empty
                                : reader["MAMH"].ToString()!,



                                TenMh =
                                reader["TENMH"] == DBNull.Value
                                ? string.Empty
                                : reader["TENMH"].ToString()!,



                                SoTinChi =
                                reader["SOTINCHI"] == DBNull.Value
                                ? 0
                                : Convert.ToInt32(reader["SOTINCHI"]),



                                MaGv =
                                reader["MAGV"] == DBNull.Value
                                ? string.Empty
                                : reader["MAGV"].ToString()!,



                                TenGiangVien =
                                reader["TENGIANGVIEN"] == DBNull.Value
                                ? null
                                : reader["TENGIANGVIEN"].ToString(),



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
                                Convert.ToBoolean(reader["HUYLOP"])

                            });

                        }

                    }

                }

            }



            if (list.Count == 0)
            {
                return NotFound(new APIResponse<object>
                {
                    Message = $"Giảng viên {maGv} chưa được phân công lớp tín chỉ.",
                    Data = null
                });
            }



            return Ok(new APIResponse<List<LTC_GVModel>>
            {
                Message = "Lấy danh sách lớp tín chỉ của giảng viên thành công.",
                Data = list
            });
        }
    }
}
