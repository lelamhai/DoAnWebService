namespace DoAnWebService.DTO.Teacher
{
    public class DiemTheoGVModel
    {
        public int Stt { get; set; }

        public int MaLtc { get; set; }

        public string MaSv { get; set; } = string.Empty;

        public string MaMh { get; set; } = string.Empty;

        public string TenMh { get; set; } = string.Empty;

        public int SoTinChi { get; set; }

        public int HocKy { get; set; }

        public string NienKhoa { get; set; } = string.Empty;

        public decimal? DiemCc { get; set; }

        public decimal? DiemGk { get; set; }

        public decimal? DiemCk { get; set; }

        public decimal? DiemTong { get; set; }

        public string XepLoai { get; set; } = string.Empty;
    }
}
