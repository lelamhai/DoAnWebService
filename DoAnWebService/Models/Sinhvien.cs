using System.Text.Json.Serialization;

namespace DoAnWebService.Models;

public partial class Sinhvien
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

    public string? Password { get; set; }

    [JsonIgnore]
    public virtual ICollection<Dangky> Dangkies { get; set; } = new List<Dangky>();
    [JsonIgnore]
    public virtual ICollection<Hocphi> Hocphis { get; set; } = new List<Hocphi>();
    [JsonIgnore]
    public virtual Lop MalopNavigation { get; set; } = null!;
}
