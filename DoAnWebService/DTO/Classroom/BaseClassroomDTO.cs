namespace DoAnWebService.DTO.Lop
{
    public class BaseClassroomDTO
    {
        public string Tenlop { get; set; } = null!;

        public string Khoahoc { get; set; } = null!;

        public string Makhoa { get; set; } = null!;

        public string Manv { get; set; } = null!;

        public DateOnly Ngaymolop { get; set; }

        public int Trangthai { get; set; }
    }
}
