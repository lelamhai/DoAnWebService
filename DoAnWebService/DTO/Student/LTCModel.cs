namespace DoAnWebService.DTO.Student
{
    public class LTCModel
    {
        public int MaLtc { get; set; }

        public string NienKhoa { get; set; } = string.Empty;

        public int HocKy { get; set; }

        public string MaMh { get; set; } = string.Empty;

        public string TenMh { get; set; } = string.Empty;

        public int SoTinChi { get; set; }

        public int SoTietLt { get; set; }

        public int SoTietTh { get; set; }

        public string? MaGv { get; set; }

        public string? TenGiangVien { get; set; }

        public int SiSoHienTai { get; set; }

        public int SiSoToiDa { get; set; }

        public string SiSo { get; set; } = string.Empty;

        public string DayThuTrongTuan { get; set; } = string.Empty;

        public string LichHoc { get; set; } = string.Empty;

        public DateTime? ThoiGianBatDau { get; set; }

        public DateTime? ThoiGianKetThuc { get; set; }

        public string ThoiGianHoc { get; set; } = string.Empty;

        public bool HuyLop { get; set; }
    }
}
