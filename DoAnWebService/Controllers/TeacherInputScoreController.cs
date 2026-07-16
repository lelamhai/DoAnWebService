using DoAnWebService.DTO.Student;
using DoAnWebService.DTO.Teacher;
using DoAnWebService.Utils;
using DoAnWebService.Utlis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DoAnWebService.Controllers
{
    [Route("api/v1/private/[controller]")]
    [ApiController]
    public class TeacherInputScoreController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public TeacherInputScoreController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("get-list")]
        public async Task<IActionResult> GetDanhSachSVTheoMon(string maGv,string maMh)
        {

            if (string.IsNullOrWhiteSpace(maGv))
            {
                return BadRequest(new APIResponse<object>
                {
                    Message = "Mã giảng viên không được để trống.",
                    Data = null
                });
            }


            if (string.IsNullOrWhiteSpace(maMh))
            {
                return BadRequest(new APIResponse<object>
                {
                    Message = "Mã môn học không được để trống.",
                    Data = null
                });
            }



            List<DanhSachSVMonGVModel> list = new();



            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection")))
            {

                using (SqlCommand cmd = new SqlCommand(
                    "SP_LAYDS_SV_THEO_GV_MON",
                    conn))
                {

                    cmd.CommandType = CommandType.StoredProcedure;



                    cmd.Parameters.Add("@MAGV",
                        SqlDbType.VarChar, 20)
                        .Value = maGv.Trim();



                    cmd.Parameters.Add("@MAMH",
                        SqlDbType.VarChar, 20)
                        .Value = maMh.Trim();



                    await conn.OpenAsync();



                    using (SqlDataReader reader =
                        await cmd.ExecuteReaderAsync())
                    {

                        while (await reader.ReadAsync())
                        {

                            list.Add(new DanhSachSVMonGVModel
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




                                MaSv =
                                reader["MASV"] == DBNull.Value
                                ? string.Empty
                                : reader["MASV"].ToString()!,



                                Ho =
                                reader["HO"] == DBNull.Value
                                ? string.Empty
                                : reader["HO"].ToString()!,



                                Ten =
                                reader["TEN"] == DBNull.Value
                                ? string.Empty
                                : reader["TEN"].ToString()!,



                                HoTenSv =
                                reader["HOTENSV"] == DBNull.Value
                                ? string.Empty
                                : reader["HOTENSV"].ToString()!,




                                DiemCc =
                                reader["DIEM_CC"] == DBNull.Value
                                ? null
                                : Convert.ToDecimal(reader["DIEM_CC"]),



                                DiemGk =
                                reader["DIEM_GK"] == DBNull.Value
                                ? null
                                : Convert.ToDecimal(reader["DIEM_GK"]),



                                DiemCk =
                                reader["DIEM_CK"] == DBNull.Value
                                ? null
                                : Convert.ToDecimal(reader["DIEM_CK"])

                            });

                        }

                    }

                }

            }



            if (list.Count == 0)
            {
                return NotFound(new APIResponse<object>
                {
                    Message =
                    $"Không tìm thấy sinh viên của môn {maMh} do giảng viên {maGv} phụ trách.",
                    Data = null
                });
            }



            return Ok(new APIResponse<List<DanhSachSVMonGVModel>>
            {
                Message = "Lấy danh sách sinh viên thành công.",
                Data = list
            });

        }

        [HttpPut("update-score")]
        public async Task<IActionResult> CapNhatDiemSV(CapNhatDiemSVModel model)
        {

            if (model == null)
            {
                return BadRequest(new APIResponse<object>
                {
                    Message = "Dữ liệu không được để trống.",
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


            if (string.IsNullOrWhiteSpace(model.MaSv))
            {
                return BadRequest(new APIResponse<object>
                {
                    Message = "Mã sinh viên không được để trống.",
                    Data = null
                });
            }

            try
            {

                using (SqlConnection conn = new SqlConnection(
                    _configuration.GetConnectionString("DefaultConnection")))
                {

                    using (SqlCommand cmd = new SqlCommand(
                        "SP_CAPNHAT_DIEM_SV",
                        conn))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;



                        cmd.Parameters.Add("@MALTC",
                            SqlDbType.Int)
                            .Value = model.MaLtc;



                        cmd.Parameters.Add("@MASV",
                            SqlDbType.VarChar, 20)
                            .Value = model.MaSv.Trim();



                        cmd.Parameters.Add("@DIEM_CC",
                            SqlDbType.Decimal)
                            .Value = model.DiemCc;



                        cmd.Parameters["@DIEM_CC"].Precision = 4;
                        cmd.Parameters["@DIEM_CC"].Scale = 1;



                        cmd.Parameters.Add("@DIEM_GK",
                            SqlDbType.Decimal)
                            .Value = model.DiemGk;



                        cmd.Parameters["@DIEM_GK"].Precision = 4;
                        cmd.Parameters["@DIEM_GK"].Scale = 1;



                        cmd.Parameters.Add("@DIEM_CK",
                            SqlDbType.Decimal)
                            .Value = model.DiemCk;



                        cmd.Parameters["@DIEM_CK"].Precision = 4;
                        cmd.Parameters["@DIEM_CK"].Scale = 1;



                        await conn.OpenAsync();



                        int result = await cmd.ExecuteNonQueryAsync();



                        
                            return Ok(new APIResponse<object>
                            {
                                Message = "Cập nhật điểm thành công.",
                                Data = null
                            });

                    }

                }

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
    }
}
