using System.Text.Json.Serialization;

namespace DoAnWebService.Models;

public partial class Nhanvien
{
    public string Manv { get; set; } = null!;

    public string Ho { get; set; } = null!;

    public string Ten { get; set; } = null!;

    public bool Phai { get; set; }

    public string? Diachi { get; set; }

    public DateOnly? Ngaysinh { get; set; }

    public string? Email { get; set; }

    public bool Danglam { get; set; }

    public string? Password { get; set; }

    public int MaLoaiNv { get; set; }
    [JsonIgnore]
    public virtual ICollection<Lop> Lops { get; set; } = new List<Lop>();
    [JsonIgnore]
    public virtual Loainhanvien MaLoaiNvNavigation { get; set; } = null!;
}
