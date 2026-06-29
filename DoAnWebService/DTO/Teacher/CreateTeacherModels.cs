namespace DoAnWebService.DTO.Teacher
{
    public class CreateTeacherModels
    {
        public string Magv { get; set; } = string.Empty;
        public string Makhoa { get; set; } = string.Empty;
        public string Ho { get; set; } = string.Empty;
        public string Ten { get; set; } = string.Empty;
        public bool? Phai { get; set; }
        public string? Diachi { get; set; }
        public string? Sodienthoai { get; set; }
        public string? Ngaysinh { get; set; }
        public string? Email { get; set; }
        public string? Hocvi { get; set; }
        public string? Hocham { get; set; }
        public string? Chuyenmon { get; set; }
        public int Trangthai { get; set; }
    }
}
