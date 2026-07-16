namespace DoAnWebService.DTO.Teacher
{
    public class LTC_GVModel
    {
        public int MaLtc { get; set; }

        public string NienKhoa { get; set; } = string.Empty;

        public int HocKy { get; set; }


        public string MaMh { get; set; } = string.Empty;

        public string TenMh { get; set; } = string.Empty;

        public int SoTinChi { get; set; }


        public string MaGv { get; set; } = string.Empty;

        public string? TenGiangVien { get; set; }


        public int SiSoToiDa { get; set; }

        public string? DayThuTrongTuan { get; set; }


        public DateTime? ThoiGianBatDau { get; set; }

        public DateTime? ThoiGianKetThuc { get; set; }


        public bool HuyLop { get; set; }
    }
}
