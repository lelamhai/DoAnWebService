namespace DoAnWebService.DTO.Teacher
{
    public class NhapDiemGVModel
    {
        public int MaLtc { get; set; }

        public string NienKhoa { get; set; } = string.Empty;

        public int HocKy { get; set; }

        public string MaMh { get; set; } = string.Empty;

        public string MaSv { get; set; } = string.Empty;


        public double? DiemCc { get; set; }

        public double? DiemGk { get; set; }

        public double? DiemCk { get; set; }


        public bool HuyDangKy { get; set; }
    }
}
