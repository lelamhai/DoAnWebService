namespace DoAnWebService.DTO.Student
{
    public class UpdateStudent
    {
        public string Ho { get; set; } = null!;

        public string Ten { get; set; } = null!;

        public bool Phai { get; set; }

        public string Diachi { get; set; }

        public string Sodienthoai { get; set; }

        public string Ngaysinh { get; set; }

        public string Email { get; set; }

        public string Malop { get; set; } = null!;

        public int Trangthai { get; set; }
    }
}
