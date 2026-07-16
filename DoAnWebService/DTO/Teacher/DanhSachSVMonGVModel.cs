namespace DoAnWebService.DTO.Teacher
{
    public class DanhSachSVMonGVModel
    {
        // Thông tin lớp tín chỉ
        public int MaLtc { get; set; }

        public string NienKhoa { get; set; } = string.Empty;

        public int HocKy { get; set; }


        // Thông tin môn học
        public string MaMh { get; set; } = string.Empty;

        public string TenMh { get; set; } = string.Empty;

        public int SoTinChi { get; set; }


        // Thông tin sinh viên
        public string MaSv { get; set; } = string.Empty;

        public string Ho { get; set; } = string.Empty;

        public string Ten { get; set; } = string.Empty;

        public string HoTenSv { get; set; } = string.Empty;


        // Điểm
        public decimal? DiemCc { get; set; }

        public decimal? DiemGk { get; set; }

        public decimal? DiemCk { get; set; }
    }
}
