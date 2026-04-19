namespace DoAnWebService.DTO.Sinhvien
{
    public class BaseStudentDTO
    {
        public string Masv { get; set; } = null!;
        public string Ho { get; set; } = null!;

        public string Ten { get; set; } = null!;

        public bool Phai { get; set; }

        public string? Diachi { get; set; }

        public DateOnly? Ngaysinh { get; set; }

        public string? Email { get; set; }

        public string Malop { get; set; } = null!;

        public bool Danghoc { get; set; }
    }
}
