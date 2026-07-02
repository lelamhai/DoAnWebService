namespace DoAnWebService.DTO.Student
{
    public class RegisterLTCModel
    {
        public int MaLtc { get; set; }

        public string MaMh { get; set; } = string.Empty;

        public string TenMh { get; set; } = string.Empty;

        public int SoTinChi { get; set; }

        public string? MaGv { get; set; }

        public string? TenGiangVien { get; set; }

        public string NienKhoa { get; set; } = string.Empty;

        public int HocKy { get; set; }

        public string DayThuTrongTuan { get; set; } = string.Empty;

        public string LichHoc { get; set; } = string.Empty;

        public DateTime? ThoiGianBatDau { get; set; }

        public DateTime? ThoiGianKetThuc { get; set; }

        public string ThoiGianHoc { get; set; } = string.Empty;
    }
}
