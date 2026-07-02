namespace DoAnWebService.DTO.Employment
{
    public class CreateLTCModel
    {
        public string NienKhoa { get; set; } = string.Empty;
        public int HocKy { get; set; }
        public string MaMH { get; set; } = string.Empty;
        public string MaGV { get; set; } = string.Empty;
        public int SiSoToiDa { get; set; }
        public string DayThuTrongTuan { get; set; } = string.Empty;
        public string ThoiGianBatDau { get; set; }
        public string ThoiGianKetThuc { get; set; }
        public bool HuyLop { get; set; } = false;
    }
}
