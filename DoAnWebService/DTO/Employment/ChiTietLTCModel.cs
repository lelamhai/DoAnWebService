namespace DoAnWebService.DTO.Employment
{
    public class ChiTietLTCModel
    {
        // Lớp tín chỉ
        public int MaLtc { get; set; }

        public string NienKhoa { get; set; } = string.Empty;

        public int HocKy { get; set; }

        public int SiSoToiDa { get; set; }

        public string? DayThuTrongTuan { get; set; }

        public DateTime? ThoiGianBatDau { get; set; }

        public DateTime? ThoiGianKetThuc { get; set; }

        public bool HuyLop { get; set; }



        // Môn học
        public string MaMh { get; set; } = string.Empty;

        public string TenMh { get; set; } = string.Empty;

        public int SoTietLt { get; set; }

        public int SoTietTh { get; set; }

        public int SoTinChi { get; set; }



        // Giảng viên
        public string MaGv { get; set; } = string.Empty;

        public string TenGiangVien { get; set; } = string.Empty;

        public string? Email { get; set; }

        public string? SoDienThoai { get; set; }

        public string? HocVi { get; set; }

        public string? HocHam { get; set; }

        public string? ChuyenMon { get; set; }
    }
}
