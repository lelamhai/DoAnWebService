using DoAnWebService.Models;
using System.Text.Json.Serialization;

namespace DoAnWebService.DTO.Classroom
{
    public class ClassroomModel
    {
        public string Malop { get; set; } = null!;

        public string Tenlop { get; set; } = null!;

        public string Khoahoc { get; set; } = null!;

        public string Tenkhoa { get; set; } = null!;

        public string Tennv { get; set; } = null!;

        public string Ngaymolop { get; set; }

        public string Trangthai { get; set; }
    }
}
