using DoAnWebService.DTO.Employment;
using DoAnWebService.DTO.Teacher;
using DoAnWebService.Utlis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DoAnWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PointController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public PointController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("get-list")]
        public async Task<IActionResult> List(string maSv)
        {
            if (string.IsNullOrWhiteSpace(maSv))
            {
                return BadRequest(new APIResponse<object>
                {
                    Message = "Mã sinh viên không được để trống.",
                    Data = null
                });
            }



            List<DiemSVModel> list = new();



            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection")))
            {

                using (SqlCommand cmd = new SqlCommand(
                    "SP_XEMDIEM_SV",
                    conn))
                {

                    cmd.CommandType = CommandType.StoredProcedure;



                    cmd.Parameters.Add("@MASV",
                        SqlDbType.VarChar, 20)
                        .Value = maSv.Trim();



                    await conn.OpenAsync();



                    using (SqlDataReader reader =
                        await cmd.ExecuteReaderAsync())
                    {

                        while (await reader.ReadAsync())
                        {

                            list.Add(new DiemSVModel
                            {

                                MaSv =
                                reader["MASV"] == DBNull.Value
                                ? string.Empty
                                : reader["MASV"].ToString()!,



                                NienKhoa =
                                reader["NIENKHOA"] == DBNull.Value
                                ? string.Empty
                                : reader["NIENKHOA"].ToString()!,



                                HocKy =
                                reader["HOCKY"] == DBNull.Value
                                ? 0
                                : Convert.ToInt32(reader["HOCKY"]),



                                MaLtc =
                                reader["MALTC"] == DBNull.Value
                                ? 0
                                : Convert.ToInt32(reader["MALTC"]),



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



                                DiemCc =
                                reader["DIEM_CC"] == DBNull.Value
                                ? 0
                                : Convert.ToDouble(reader["DIEM_CC"]),



                                DiemGk =
                                reader["DIEM_GK"] == DBNull.Value
                                ? 0
                                : Convert.ToDouble(reader["DIEM_GK"]),



                                DiemCk =
                                reader["DIEM_CK"] == DBNull.Value
                                ? 0
                                : Convert.ToDouble(reader["DIEM_CK"]),



                                DiemTongKet =
                                reader["DIEM_TONGKET"] == DBNull.Value
                                ? 0
                                : Convert.ToDouble(reader["DIEM_TONGKET"]),



                                XepLoai =
                                reader["XEPLOAI"] == DBNull.Value
                                ? string.Empty
                                : reader["XEPLOAI"].ToString()!

                            });

                        }

                    }

                }

            }



            if (list.Count == 0)
            {
                return NotFound(new APIResponse<object>
                {
                    Message = $"Không tìm thấy điểm của sinh viên {maSv}.",
                    Data = null
                });
            }



            return Ok(new APIResponse<List<DiemSVModel>>
            {
                Message = "Lấy điểm sinh viên thành công.",
                Data = list
            });
        }


        [HttpPut("update/{maltc}")]
        public async Task<IActionResult> Update(int maltc, CapNhatDiemModel model)
        {
            if (model == null)
            {
                return BadRequest(new APIResponse<object>
                {
                    Message = "Dữ liệu không được để trống.",
                    Data = null
                });
            }


            if (maltc <= 0)
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
                        "SP_CAPNHAT_DIEM_LTC",
                        conn))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;



                        cmd.Parameters.Add("@MALTC",
                            SqlDbType.Int)
                            .Value = maltc;



                        cmd.Parameters.Add("@DIEM_CC",
                            SqlDbType.Float)
                            .Value = model.DiemCc;



                        cmd.Parameters.Add("@DIEM_GK",
                            SqlDbType.Float)
                            .Value = model.DiemGk;



                        cmd.Parameters.Add("@DIEM_CK",
                            SqlDbType.Float)
                            .Value = model.DiemCk;



                        await conn.OpenAsync();



                        using (SqlDataReader reader =
                            await cmd.ExecuteReaderAsync())
                        {

                            if (await reader.ReadAsync())
                            {

                                return Ok(new APIResponse<object>
                                {
                                    Message = "Cập nhật điểm thành công.",
                                    Data = null
                                });

                            }

                        }

                    }

                }



                return BadRequest(new APIResponse<object>
                {
                    Message = "Không cập nhật được điểm.",
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
    }
}
