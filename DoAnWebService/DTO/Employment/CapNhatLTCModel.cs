namespace DoAnWebService.DTO.Employment
{
    public class CapNhatLTCModel
    {
        public int MaLtc { get; set; }

        public string NienKhoa { get; set; } = string.Empty;

        public int HocKy { get; set; }

        public string MaMh { get; set; } = string.Empty;

        public string MaGv { get; set; } = string.Empty;

        public int SiSoToiDa { get; set; }

        public string DayThuTrongTuan { get; set; } = string.Empty;

        public DateTime ThoiGianBatDau { get; set; }

        public DateTime ThoiGianKetThuc { get; set; }

        public bool HuyLop { get; set; }
    }
}
